using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Pickup : MonoBehaviour, IInteractable
{
    public AudioSource source;
    private Item _item;

    private void Start()
    {
        _item = GetComponent<Item>();
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
