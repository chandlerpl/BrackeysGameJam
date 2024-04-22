using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(UniqueID))]
public class Grid : MonoBehaviour
{
    private Collider col;
    private UniqueID uniqueID;

    public uint UniqueID => uniqueID.ID;
    private void Start()
    {
        col = GetComponent<Collider>();
        uniqueID = GetComponent<UniqueID>();

        col.isTrigger = true;

        GameManager.Instance.GridManager.AddGrid(this);
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 min = col.bounds.min;
        Vector3 max = col.bounds.max;

        float x = Random.Range(min.x, max.x);
        float y = min.y;
        float z = Random.Range(min.z, max.z);

        return new Vector3(x, y, z);
    }
}
