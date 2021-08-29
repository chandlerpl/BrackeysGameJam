using CP.AILibrary.FiniteStateMachine;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "WaitAction", menuName = "CP/Action/Wait")]
public class WaitAction : Action
{
    public Vector3 startPosition;

    public override void Act(StateMachine stateMachine)
    {

    }

    public override void Enter(StateMachine stateMachine)
    {

    }

    public override void Exit(StateMachine stateMachine)
    {
        stateMachine.GetComponent<NavMeshAgent>().enabled = true;
        stateMachine.GetComponent<NavMeshAgent>().Warp(startPosition);
    }
}
