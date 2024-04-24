using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Dictionary<uint, Grid> grids = new Dictionary<uint, Grid>();

    private List<uint> gridList = new List<uint>();
    public void AddGrid(Grid grid)
    {
        grids.Add(grid.UniqueID, grid);
        gridList.Add(grid.UniqueID);
    }

    public Grid GetRandomGrid()
    {
        return grids[gridList[Random.Range(0, gridList.Count)]];
    }


    public Grid GetRandomGrid(List<uint> extraGrids)
    {
        List<uint> searchGrids = new List<uint>(gridList);
        searchGrids.AddRange(extraGrids);

        return grids[searchGrids[Random.Range(0, searchGrids.Count)]];
    }
}
