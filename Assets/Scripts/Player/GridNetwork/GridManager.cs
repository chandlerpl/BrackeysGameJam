using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Dictionary<uint, Grid> grids = new Dictionary<uint, Grid>();

    public void AddGrid(Grid grid)
    {
        grids.Add(grid.UniqueID, grid);
    }
}
