using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemIdentifier;
    public int quantity = 1;
    public GameObject itemIconPrefab;

    public Vector3 uiPosition;
    public Vector3 uiRotation;

    public Rigidbody Rigidbody { get; private set; }
    public Collider Collider { get; private set; }

    private void Awake()
    {
        if(TryGetComponent(out Rigidbody rb))
        {
            Rigidbody = rb;
            Rigidbody.isKinematic = true;
        }

        Collider = GetComponent<Collider>();
    }
}
