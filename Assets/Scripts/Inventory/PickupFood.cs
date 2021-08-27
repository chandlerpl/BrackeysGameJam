using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class PickupFood : MonoBehaviour, IInteractable
{
    public AudioSource source;

    public void OnInteract(GameObject interactingObj, PlayerInventory inventory)
    {
        Item item = GetComponent<Item>();

        if(ShoppingManager.Instance != null && ShoppingManager.Instance.IsRequiredItem(item))
        {
            inventory.AddItem(item);
            ShoppingManager.Instance.CheckOffItem(item);
            
              
            gameObject.SetActive(false);

             if (source != null)
             {
                 source.Play();
             }
        }
    }
}
