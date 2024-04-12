public class Inverter<T> : Node<T>
{
    public Node<T> node;

    protected override NodeState Evaluate(T data)
    {
        NodeState state = node.Eval(data);

        if (state == NodeState.Running)
            return NodeState.Running;

        return state == NodeState.Success ? NodeState.Failure : NodeState.Success;
    }

    public override bool Init(BehaviourTree<T> behaviourTree)
    {
        return node.Init(behaviourTree);
    }

    protected override void UpdateIK(T data)
    {
        node.EvalIK(data);
    }
}
