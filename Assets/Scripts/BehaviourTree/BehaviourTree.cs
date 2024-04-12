using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree<T>
{
    private Node<T> root;

    public BehaviourTree(Node<T> treeRoot)
    {
        root = treeRoot;
    }

    public void Update(T data)
    {
        root.Eval(data);
    }

    public void UpdateIK(T data)
    {
        root.EvalIK(data);
    }
}
