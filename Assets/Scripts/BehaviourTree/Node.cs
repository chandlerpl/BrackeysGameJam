using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node<T>
{
    public NodeState NodeState { get; protected set; }

    public abstract bool Init(BehaviourTree<T> tree);

    public NodeState Eval(T data)
    {
        NodeState = Evaluate(data);

        return NodeState;
    }
    public void EvalIK(T data)
    {
        UpdateIK(data);
    }

    protected abstract NodeState Evaluate(T data);

    protected virtual void UpdateIK(T data) { }
}

public enum NodeState
{
    Success, Failure, Running
}