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
}
