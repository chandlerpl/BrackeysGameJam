using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public GridObjectType type;

    public Grid CurrentGrid { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Grid grid))
        {
            CurrentGrid = grid;

            Debug.Log("Entering new grid: " + CurrentGrid.UniqueID);
            
        }
    }
}

public enum GridObjectType
{
    Player, AIAgent, Trap
}