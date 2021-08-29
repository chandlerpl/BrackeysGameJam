using CP.AILibrary.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CaughtDecision", menuName = "CP/Decision/Caught")]
public class CaughtDecision : Decision
{
    public float catchDistance = 0.5f;

    public override bool Decide(StateMachine stateMachine)
    {
        if(!stateMachine.memory.CheckDataExist("currentTarget"))
        {
            return false;
        }

        return Vector3.Distance(stateMachine.transform.position, stateMachine.memory.GetData<Vector3>("currentTarget").value) < catchDistance;
    }
}
