using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridObject))]
[RequireComponent(typeof(UniqueID))]
public class TrapTrigger : MonoBehaviour
{
    public List<Trap> traps = new List<Trap>();

    private UniqueID uniqueID;

    public uint UniqueID => uniqueID.ID;

    protected void Start()
    {
        uniqueID = GetComponent<UniqueID>();

        GameManager.Instance.TrapManager.AddTrap(this);
    }

    public void Trigger()
    {
        foreach(Trap trap in traps)
        {
            trap.Enable();
        }
    }
}
