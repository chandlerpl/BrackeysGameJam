using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    public string itemIdentifier;
    public int quantity = 1;
    public GameObject itemIconPrefab;
    public Rigidbody Rigidbody { get; private set; }
    public Collider Collider { get; private set; }

    private void Start()
    {
        if(TryGetComponent(out Rigidbody rb))
        {
            Rigidbody = rb;
            Rigidbody.isKinematic = true;
        }

        Collider = GetComponent<Collider>();
    }
}
