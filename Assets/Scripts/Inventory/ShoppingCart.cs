using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            if(other.TryGetComponent(out Item item))
            {
                ShoppingManager.Instance.CheckOffItem(item);
            }
        }
    }
}
