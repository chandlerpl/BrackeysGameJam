using CP.AILibrary.Storage;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualDetectionNode : Node<AIMovement>
{
    public int tickOffset = 5;
    public float detectionRange = 10;
    public float detectionTime = 1; // Seconds
    public float fov = 160; // Degrees

    private int _currentTick = 0;
    private float _timeSinceLastCheck = 0;
    private Dictionary<Detectable, float> detectingUnits = new Dictionary<Detectable, float>();

    private List<Detectable> detectedUnits = new List<Detectable>();
    List<Detectable> updatedUnits = new List<Detectable>();
    public override bool Init(BehaviourTree<AIMovement> tree)
    {
        return true;
    }

    
    protected override NodeState Evaluate(AIMovement data)
    {
        _timeSinceLastCheck += Time.deltaTime;

        // Updates previously detected objects until no longer tracked, will provide last seen location of entity.
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
                    detectedUnits.Remove(detectable);
                    data.chasedPlayer = null;
                }
            } else
            {
                if(detectedUnits.Contains(detectable))
                {
                    data.Agent.SetDestination(detectable.transform.position);
                    data.chasedPlayer = detectable.transform;
                }
            }
        }

        // Slow down visual perception of AI to prevent major dips in framerate from Vision searches.
        if (++_currentTick < tickOffset)
        {
            // if chasing a detectable then make sure it continues running after them
            return detectedUnits.Count > 0 ? NodeState.Success : NodeState.Failure;
        }
        _currentTick = 0;

        // Searches the Detectable layer for any objects in range.
        Collider[] cols = Physics.OverlapSphere(data.VisionLocation.position, detectionRange, data.DetectionMask);

        updatedUnits.Clear();
        float clostestAgent = float.MaxValue;
        foreach (Collider col in cols)
        {
            if (!col.gameObject.TryGetComponent<Detectable>(out var detectable))
                continue;

            if (detectable.type == EntityTypes.Player)
            {
                if (Vector3.Angle(col.transform.position - data.VisionLocation.position, data.VisionLocation.forward) <= fov / 2)
                {
                    float dist = TryDetect(data.VisionLocation.position, detectable, data);
                    if (dist > -1000)
                    {
                        updatedUnits.Add(detectable);
                        if (detectingUnits.ContainsKey(detectable))
                        {
                            float currentDetection = detectingUnits[detectable] + _timeSinceLastCheck;
                            float detectTime = detectionTime * (dist / detectionRange);

                            if (currentDetection >= detectTime && dist < clostestAgent)
                            {
                                clostestAgent = dist;
                                //currentDetected = detectable;
                                //memory.SetValue("currentTarget", detectable.transform.position);
                                //unitDetection[detectable] = memoryTime;

                                data.animator.SetBool("IsSprinting", true);
                                data.Agent.SetDestination(col.transform.position);

                                data.chasedPlayer = col.transform;

                                if (!detectedUnits.Contains(detectable))
                                {
                                    detectedUnits.Add(detectable);
                                }
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
        if(detectedUnits.Count > 0)
        {
            return NodeState.Success;
        }

        data.animator.SetBool("IsSprinting", false);
        return NodeState.Failure;
    }

    private float TryDetect(Vector3 location, Detectable detectable, AIMovement data)
    {
        foreach (Vector3 vec in detectable.spottableLocations)
        {
            if (Physics.Linecast(location, detectable.transform.position + vec, out RaycastHit rayHit, data.LOSMask) && rayHit.collider.gameObject == detectable.gameObject)
            {
                return rayHit.distance;
            }
        }

        return -1000;
    }
}
