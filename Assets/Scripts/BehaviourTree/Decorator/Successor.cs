
public class Successor<T> : Node<T>
{
    public Node<T> node;

    protected override NodeState Evaluate(T data)
    {
        NodeState result = node.Eval(data);

        return result == NodeState.Running ? result : NodeState.Success;
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
