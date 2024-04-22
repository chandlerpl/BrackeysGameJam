using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGridNode : Node<AIMovement>
{
    public override bool Init(BehaviourTree<AIMovement> tree)
    {
        return true;
    }

    protected override NodeState Evaluate(AIMovement data)
    {
        data.Agent.SetDestination(GameManager.Instance.GridManager.GetRandomGrid().GetRandomPosition());

        return NodeState.Success;
    }
}
