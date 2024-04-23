using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrap : Trap
{
    public GameObject lightOn;
    public GameObject lightOff;
    public HingeJoint hinge;
    public float min;
    public float max;

    protected override void DisableTrap()
    {
        //throw new System.NotImplementedException();
    }

    protected override void EnableTrap()
    {
        lightOn.SetActive(false);
        lightOff.SetActive(true);

        JointLimits limit = hinge.limits;
        limit.min = min;
        limit.max = max;
        hinge.limits = limit;
    }
}
