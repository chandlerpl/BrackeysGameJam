using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHint : MonoBehaviour
{
    public IKPosition position;
    public Vector3 offset;
    public Vector3 rotationOffset;
}

public enum IKPosition
{
    LeftArm,
    RightArm,
    LeftIndex,
    RightIndex,
    LeftFinger,
    RightFinger,
    LeftThumb,
    RightThumb,
}