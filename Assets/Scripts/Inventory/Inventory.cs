using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private float interactRange;
    [SerializeField]
    private int maxItems = 1;
    [SerializeField]
    private float throwForce = 2f;

    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform handPosition;

    private Item[] inventory;
    private int _inventorySlot = 0;
    public void Interact()
    {
        if (inventory[_inventorySlot] != null)
        {
            RemoveItem(inventory[_inventorySlot], _inventorySlot);
            return;
        }


        if (Physics.SphereCast(cam.position, 0.25f, cam.forward, out RaycastHit hit, interactRange, LayerMask.GetMask("Interactable")))
        {
/*            if (hit.collider.TryGetComponent<Tooltip>(out var tooltip))
                tooltipText.text = tooltip.tooltip;*/

            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.OnInteract(gameObject, this);
            }
        }
        /* else
        {
            tooltipText.text = "";
        }*/
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;

                item.transform.SetParent(handPosition);
                item.transform.localPosition = Vector3.zero;
                
                if(item.TryGetComponent(out Pickup pickup))
                {
                    pickup.Rigidbody.isKinematic = true;
                }

                // Add item to hand?
                //item.gameObject.SetActive(false);

                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(Item item, int slot)
    {
        inventory[slot] = null;

        item.transform.parent = null;
        item.gameObject.SetActive(true);

        if (item.TryGetComponent(out Pickup pickup))
        {
            pickup.Rigidbody.isKinematic = false;
            pickup.Rigidbody.AddForce(cam.forward * throwForce, ForceMode.Impulse);
        }

        return true;
    }

    public bool ContainsItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == item)
            {
                return true;
            }
        }

        return false;
    }

    void Start()
    {
        inventory = new Item[maxItems];
    }
}
