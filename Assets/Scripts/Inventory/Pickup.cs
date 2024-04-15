using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour, IInteractable
{
    public AudioSource source;
    private Item _item;
    public Rigidbody Rigidbody { get; private set; }

    private void Start()
    {
        _item = GetComponent<Item>();
        Rigidbody = GetComponent<Rigidbody>();

        Rigidbody.isKinematic = true;
    }

    public void OnInteract(GameObject interactingObj, Inventory inventory)
    {
        if (inventory.AddItem(_item))
        {
            //gameObject.SetActive(false);

            if (source != null)
            {
                source.Play();
            }
        }
    }
}
