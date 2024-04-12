using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualDetectionNode : Node<AIMovement>
{
    public int tickOffset = 5;
    public float detectionRange = 10;
    public float detectionTime = 2; // Seconds
    public float fov = 90; // Seconds

    private int _currentTick = 0;
    private float _timeSinceLastCheck = 0;
    private Dictionary<Detectable, float> detectingUnits = new Dictionary<Detectable, float>();

    List<Detectable> updatedUnits = new List<Detectable>();
    public override bool Init(BehaviourTree<AIMovement> tree)
    {
        return true;
    }

    
    protected override NodeState Evaluate(AIMovement data)
    {
        _timeSinceLastCheck += Time.deltaTime;
        foreach (Detectable detectable in detectingUnits.Keys.ToArray())
        {
            if (!updatedUnits.Contains(detectable))
            {
                float detection = detectingUnits[detectable] - Time.deltaTime;

                if (detection > 0)
                {
                    detectingUnits[detectable] = detection;
                }
                else
                {
                    detectingUnits.Remove(detectable);
                }
            }
        }

        if (++_currentTick < tickOffset)
        {
            return NodeState.Failure;
        }
        _currentTick = 0;

        Collider[] cols = Physics.OverlapSphere(data.transform.position, detectionRange, LayerMask.GetMask("Detectable"));

        updatedUnits.Clear();
        foreach (Collider col in cols)
        {
            Detectable detectable = col.gameObject.GetComponent<Detectable>();
            if (detectable == null)
                continue;

            if (detectable.type == EntityTypes.Player)
            {
                if (Vector3.Angle(col.transform.position - data.transform.position, data.transform.forward) <= fov / 2)
                {
                    if (Physics.Linecast(data.transform.position, col.transform.position, out RaycastHit rayHit) && rayHit.collider == col)
                    {
                        updatedUnits.Add(detectable);
                        if (detectingUnits.ContainsKey(detectable))
                        {
                            float currentDetection = detectingUnits[detectable] + _timeSinceLastCheck;
                            if (currentDetection >= detectionTime)
                            {
                                //currentDetected = detectable;
                                //memory.SetValue("currentTarget", detectable.transform.position);
                                //unitDetection[detectable] = memoryTime;

                                data.Agent.SetDestination(col.transform.position);

                                //break;
                            }
                            else
                            {
                                detectingUnits[detectable] = currentDetection;
                            }
                        }
                        else
                        {
                            detectingUnits.Add(detectable, 0);
                        }
                    }
                }
            }
        }
        _timeSinceLastCheck = 0;

        return NodeState.Failure;
    }
}
