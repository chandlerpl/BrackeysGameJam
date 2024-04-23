using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public Dictionary<uint, TrapTrigger> traps = new();

    public void AddTrap(TrapTrigger trap)
    {
        traps.Add(trap.UniqueID, trap);
        //gridList.Add(grid);
    }

    public TrapTrigger GetTrap(uint trapID)
    {
        return traps[trapID];
    }
}
