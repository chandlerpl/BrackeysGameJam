using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Dictionary<uint, Grid> grids = new Dictionary<uint, Grid>();

    private List<Grid> gridList = new List<Grid>();
    public void AddGrid(Grid grid)
    {
        grids.Add(grid.UniqueID, grid);
        gridList.Add(grid);
    }

    public Grid GetRandomGrid()
    {
        return gridList[Random.Range(0, gridList.Count)];
    }
}
