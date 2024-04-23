using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    private bool isEnabled;

    public void Enable()
    {
        if (isEnabled)
        {
            return;
        }

        isEnabled = true;
        EnableTrap();
    }

    public void Disable()
    {
        if (!isEnabled)
        {
            return;
        }

        isEnabled = false;
        DisableTrap();
    }

    protected abstract void EnableTrap();
    protected abstract void DisableTrap();
}
