using System.Collections.Generic;
using UnityEngine;

public class Sequence<T> : Node<T>
{
    public List<Node<T>> nodes = new List<Node<T>>();
    public bool isSingleSuccess = false;
    protected override NodeState Evaluate(T data)
    {
        bool childRunning = false;
        bool childSuccess = false;

        foreach (Node<T> node in nodes)
        {
            switch (node.Eval(data))
            {
                case NodeState.Failure:
                    return (isSingleSuccess && childSuccess) ? NodeState.Success : NodeState.Failure;
                case NodeState.Success:
                    continue;
                case NodeState.Running:
                    return NodeState.Running;
                default:
                    return NodeState.Success;
            }
        }
        return childRunning ? NodeState.Running : NodeState.Success;
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