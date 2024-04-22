using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKBone : MonoBehaviour
{
    public bool isTwoBone = false;
    public TwoBoneIKConstraint twoBoneConstraint;
    public ChainIKConstraint chainIKConstraint;
    public Transform Target { get => isTwoBone ? twoBoneConstraint.data.target : chainIKConstraint.data.target; }
    public float Weight { get => isTwoBone ? twoBoneConstraint.weight : chainIKConstraint.weight;
        set
        {
            if (isTwoBone)
            {
                twoBoneConstraint.weight = value;
            }
            else
            {
                chainIKConstraint.weight = value;
            }
        }
    }

    Transform parent;
    private IKHint hint;

    private void Start()
    {
        parent = Target.parent;
    }

    public void UpdatePosition(IKHint hint)
    {
        this.hint = hint;
        Target.parent = hint.transform;
        Target.localPosition = hint.offset;
        Target.localRotation = Quaternion.identity * Quaternion.Euler(hint.rotationOffset);
        Target.parent = parent;

        Weight = 1;
    }

    public void ResetPosition()
    {
        Weight = 0;
        Target.parent = parent;
    }
}
