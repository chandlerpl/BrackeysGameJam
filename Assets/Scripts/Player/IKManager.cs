using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEditor.PlayerSettings;

public class IKManager : MonoBehaviour
{
    public IKBone leftArmConstraint;
    public IKBone rightArmConstraint;

    public IKBone leftIndexConstraint;
    public IKBone leftThumbConstraint;
    public IKBone leftFingerConstraint;

    public IKBone rightIndexConstraint;
    public IKBone rightThumbConstraint;
    public IKBone rightFingerConstraint;

    public void UpdatePosition(IKHint hint)
    {
        IKBone bone = ConvertType(hint.position);

        bone.UpdatePosition(hint);
    }

    internal void ResetPosition(IKPosition position)
    {
        IKBone bone = ConvertType(position);

        bone.ResetPosition();
    }

    private IKBone ConvertType(IKPosition iKPosition)
    {
        return iKPosition switch
        {
            IKPosition.LeftArm => leftArmConstraint,
            IKPosition.RightArm => rightArmConstraint,
            IKPosition.LeftThumb => leftThumbConstraint,
            IKPosition.RightThumb => rightThumbConstraint,
            IKPosition.LeftFinger => leftFingerConstraint,
            IKPosition.RightFinger => rightFingerConstraint,
            IKPosition.LeftIndex => leftIndexConstraint,
            IKPosition.RightIndex => rightIndexConstraint,
            _ => null,
        };
    }
}
