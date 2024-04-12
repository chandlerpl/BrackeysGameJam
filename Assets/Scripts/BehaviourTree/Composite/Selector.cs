using System.Collections.Generic;
using UnityEngine;

public class Selector<T> : Node<T>
{
    public List<Node<T>> nodes = new List<Node<T>>();

    protected override NodeState Evaluate(T data)
    {
        foreach (Node<T> node in nodes)
        {
            NodeState eval = node.Eval(data);
            switch (eval)
            {
                case NodeState.Failure:
                    continue;
                case NodeState.Success:
                    return NodeState.Success;
                case NodeState.Running:
                    return NodeState.Running;
                default:
                    continue;
            }
        }

        return NodeState.Failure;
    }

    public override bool Init(BehaviourTree<T> behaviourTree)
    {
        bool returnState = true;

        foreach (Node<T> node in nodes)
        {
            if (!node.Init(behaviourTree))
                returnState = false;
        }

        return returnState;
    }

    protected override void UpdateIK(T data)
    {
        foreach (Node<T> node in nodes)
        {
            node.EvalIK(data);
        }
    }
}