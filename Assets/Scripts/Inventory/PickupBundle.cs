using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBundle : MonoBehaviour, IInteractable
{
    public AK.Wwise.Event pickupSound;
    public Item item;

    public void OnInteract(GameObject interactingObj, Inventory inventory)
    {
        GameObject go = Instantiate(item.gameObject);
        Item i = go.GetComponent<Item>();

        if (inventory.AddItem(i))
        {
            //gameObject.SetActive(false);

            pickupSound.Post(gameObject);
        }
    }
}
