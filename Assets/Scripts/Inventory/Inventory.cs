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
    private Transform cam;

    private Item[] inventory;
    public void Interact()
    {
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

                // Add item to hand?
                item.gameObject.SetActive(false);

                return true;
            }
        }

        return false;
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
