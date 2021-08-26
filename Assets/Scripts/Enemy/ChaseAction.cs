using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CP.AILibrary.FiniteStateMachine;
using CP.AILibrary.Storage;
using System.Reflection;

[CreateAssetMenu(fileName = "ChaseAction", menuName = "CP/Action/Chase")]
public class ChaseAction : Action
{ 
    public override void Act(StateMachine stateMachine)
    {
        Data<Vector3> data = stateMachine.memory.GetData<Vector3>("currentTarget");
        if (data == null)
            return;

        Vector3 val = data.value;

        NavMeshAgent agent = stateMachine.memory.GetData<NavMeshAgent>("navAgent").value;
        agent.SetDestination(val);
    }

    public override void Enter(StateMachine stateMachine)
    {
        if (!stateMachine.gameObject.TryGetComponent(out NavMeshAgent agent))
        {
            agent = stateMachine.gameObject.AddComponent<NavMeshAgent>();
        }

        stateMachine.memory.AddData("navAgent", agent);
    }

    public override void Exit(StateMachine stateMachine)
    {

    }
}
