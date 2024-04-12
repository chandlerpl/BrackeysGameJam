using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncementDecisionNode : Node<AIMovement>
{
    public override bool Init(BehaviourTree<AIMovement> tree)
    {
        return true;
    }

    protected override NodeState Evaluate(AIMovement data)
    {
        return GameManager.announcementMade ? NodeState.Success : NodeState.Failure;
    }
}
