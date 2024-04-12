using UnityEngine;

public class FindWaypointNode : Node<AIMovement>
{
    public override bool Init(BehaviourTree<AIMovement> tree)
    {
        return true;
    }

    protected override NodeState Evaluate(AIMovement data)
    {
        if(++data.CurrentWaypoint >= data.waypoints.WaypointsNumber())
        {
            data.CurrentWaypoint = 0;
        }

        data.Agent.SetDestination(data.waypoints.GetWaypoint(data.CurrentWaypoint).transform.position);

        return NodeState.Success;
    }
}

