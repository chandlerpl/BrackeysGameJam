using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            if (other.TryGetComponent(out Item item))
            {
                if (item.Rigidbody != null)
                {
                    item.Rigidbody.isKinematic = true;
                }
                item.Collider.isTrigger = true;
                item.transform.parent = transform;

                GameManager.Instance.ShoppingManager.CheckItem(item);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            if (other.TryGetComponent(out Item item))
            {
                GameManager.Instance.ShoppingManager.UncheckItem(item);
            }
        }
    }
}
