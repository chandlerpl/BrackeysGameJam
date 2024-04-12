using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CP.AILibrary.FiniteStateMachine;
using CP.AILibrary.Storage;
using System;
using UnityEditorInternal;

public class MovementNode : Node<AIMovement>
{
    public float patrolSpeed = 3.5f;

    private bool calculatingPath = false;
/*    public override void Act(StateMachine stateMachine)
    {
        Data<int> data = stateMachine.memory.GetData<int>("waypointIndex");
        if (data == null)
            return;

        int index = data.value;

        NavMeshAgent agent = stateMachine.memory.GetData<NavMeshAgent>("navAgent").value;

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !calculatingPath)
        {
            bool reverse = stateMachine.memory.GetData<bool>("waypointReverse").value;

            index += reverse ? -1 : 1;
            WaypointGroup waypoints = stateMachine.memory.GetData<WaypointGroup>("waypointData").value;
            if (index >= waypoints.WaypointsNumber() - 1 || index <= 0)
            {
                reverse = !reverse;
                stateMachine.memory.SetValue("waypointReverse", reverse);
            }

            stateMachine.memory.SetValue("waypointIndex", index);
            bool t = agent.SetDestination(waypoints.GetWaypoint(index).transform.position);
            calculatingPath = true;
        } else
        {
            calculatingPath = false;
        }
    }

    public override void Enter(StateMachine stateMachine)
    {
        agent.speed = patrolSpeed;
        WaypointGroup waypoints = stateMachine.memory.GetData<WaypointGroup>("waypointData").value;
        agent.SetDestination(waypoints.GetWaypoint(0).transform.position);
    }
*/
    public override bool Init(BehaviourTree<AIMovement> tree)
    {
        return true;
    }

    protected override NodeState Evaluate(AIMovement data)
    {
        if(data.Agent.hasPath)
        {
            data.Agent.speed = patrolSpeed;

            data.animator.SetBool("IsMoving", true);
            if (data.Agent.remainingDistance < 0.5f)
            {
                return NodeState.Success;
            } else
            {
                return NodeState.Running;
            }
        }
        if(data.Agent.pathPending)
        {
            return NodeState.Running;
        }

        data.animator.SetBool("IsMoving", false);
        return NodeState.Failure;
    }
}
