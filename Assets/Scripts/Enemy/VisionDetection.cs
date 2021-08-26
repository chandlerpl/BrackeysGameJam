using CP.AILibrary.Storage;
using CP.AILibrary.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;
using System.Linq;

[RequireComponent(typeof(StateMachine))]
public class VisionDetection : MonoBehaviour
{
    public float checkFrequency = 0.1f;
    public float fov = 90;
    public float distance = 20;
    public float detectionTime = 5;

    private Vector3 eyePosition;
    private Dictionary<Detectable, float> unitDetection = new Dictionary<Detectable, float>();
    private Memory memory;
    private Detectable currentDetected = null;

    // Start is called before the first frame update
    void Start()
    {
        eyePosition = transform.position + new Vector3(0, 0.5f, 0);

        memory = GetComponent<StateMachine>().memory;

        StartCoroutine(Detect());
    }

    IEnumerator Detect()
    {
        while(true)
        {
            yield return new WaitForSeconds(checkFrequency);

            Collider[] cols = Physics.OverlapSphere(eyePosition, distance, LayerMask.GetMask("Detectable"));

            List<Detectable> updatedUnits = new List<Detectable>();
            foreach (Collider col in cols)
            {
                Detectable detectable = col.gameObject.GetComponent<Detectable>();
                if (detectable == null)
                    continue;

                if(detectable.type == EntityTypes.Player)
                {
                    if (Vector3.Angle(col.transform.position - eyePosition, transform.forward) <= fov / 2)
                    {
                        if (Physics.Linecast(eyePosition, col.transform.position, out RaycastHit rayHit) && rayHit.collider == col)
                        {
                            updatedUnits.Add(detectable);
                            if (unitDetection.ContainsKey(detectable))
                            {
                                float currentDetection = unitDetection[detectable] + checkFrequency;
                                Debug.Log(currentDetection);
                                if (currentDetection >= detectionTime)
                                {
                                    Debug.Log("Detecting " + memory.SetValue("currentTarget", detectable.transform.position));
                                    currentDetected = detectable;
                                }
                                else
                                {
                                    unitDetection[detectable] = currentDetection;
                                }
                            } else
                            {
                                unitDetection.Add(detectable, 0);
                            }
                        }
                    }
                }
            }

            foreach (Detectable detectable in unitDetection.Keys.ToArray())
            {
                if (!updatedUnits.Contains(detectable))
                {
                    float detection = unitDetection[detectable] - 0.1f;

                    if (detection > 0)
                    {
                        unitDetection[detectable] = detection;
                    }
                    else
                    {
                        unitDetection.Remove(detectable);

                        if(currentDetected == detectable)
                        {
                            currentDetected = null;
                            memory.RemoveData("currentTarget");
                        }
                    }
                }
            }
        }
    }
}
