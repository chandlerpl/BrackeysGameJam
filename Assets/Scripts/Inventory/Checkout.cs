using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkout : MonoBehaviour, IInteractable
{
    public AK.Wwise.Event checkoutSound;

    public void OnInteract(GameObject interactingObj, Inventory inventory)
    {
        GameManager.Instance.ShoppingManager.Checkout();
        checkoutSound.Post(gameObject);
    }
}
