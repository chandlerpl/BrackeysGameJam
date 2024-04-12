using System;

public class Task<T> : Node<T>
{
    private Func<T, NodeState> action;

    public Task(Func<T, NodeState> action)
    {
        this.action = action;
    }

    public override bool Init(BehaviourTree<T> behaviourTree)
    {
        return action != null;
    }

    protected override NodeState Evaluate(T data)
    {
        return action.Invoke(data);
    }
}