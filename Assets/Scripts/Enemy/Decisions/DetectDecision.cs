using CP.AILibrary.FiniteStateMachine;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "DetectDecision", menuName = "CP/Decision/Detect")]
public class DetectDecision : Decision
{
    public override bool Decide(StateMachine stateMachine)
    {
        return stateMachine.memory.CheckDataExist("currentTarget");
    }
}
