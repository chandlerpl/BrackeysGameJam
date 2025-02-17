using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private float interactRange;
    [SerializeField]
    private int maxItems = 1;
    [SerializeField]
    private float throwForce = 2f;
    [SerializeField]
    private float dropForce = 2f;

    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform handPosition;
    [SerializeField]
    private IKManager ikManager;
    [SerializeField]
    private IKHint ikHint;

    [SerializeField]
    private AK.Wwise.Event pickupSound;
    [SerializeField]
    private AK.Wwise.Event dropSound;

    private Item[] inventory;
    private int _inventorySlot = 0;

    [SerializeField]
    private Transform pushTransform;
    public Transform PushTransform => pushTransform;

    private Pushable _currentPushable;
    public Pushable CurrentPushable { get => _currentPushable;
        set
        {
            if(value != null && _currentPushable != null)
            {
                _currentPushable.Detach(this);
            }

            _currentPushable = value;
        }
    }
    public bool PushingCart { get; set; }
    [SerializeField]
    private Transform cart;

    [SerializeField]
    private Image crosshair;
    [SerializeField]
    private Sprite defaultSprite;
    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField]
    private Sprite dropSprite;

    public void Interact()
    {
        if (inventory[_inventorySlot] != null)
        {
            RemoveItem(inventory[_inventorySlot], _inventorySlot, dropForce);
            return;
        }

        if (Physics.SphereCast(cam.position, 0.25f, cam.forward, out RaycastHit hit, interactRange, LayerMask.GetMask("Interactable")))
        {
/*            if (hit.collider.TryGetComponent<Tooltip>(out var tooltip))
                tooltipText.text = tooltip.tooltip;*/

            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.OnInteract(gameObject, this);

                crosshair.sprite = dropSprite;
            }
        }
    }

    public void Throw()
    {
        if (inventory[_inventorySlot] != null)
        {
            RemoveItem(inventory[_inventorySlot], _inventorySlot, throwForce);
            return;
        }
    }

    private void FixedUpdate()
    {
        if (Physics.SphereCast(cam.position, 0.25f, cam.forward, out RaycastHit hit, interactRange, LayerMask.GetMask("Interactable")))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                crosshair.sprite = selectedSprite;
                crosshair.enabled = true;
            }
        }
        else
        {
            crosshair.enabled = false;
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                if(PushingCart)
                {
                    //item.transform.SetParent(cart);
                    //item.transform.localPosition = new Vector3(Random.Range(-0.6f, 0.6f), 0, Random.Range(-0.9f, 0.9f));

                    if (item.Rigidbody != null)
                    {
                        item.Rigidbody.isKinematic = true;
                        item.Rigidbody.position = cart.position + new Vector3(Random.Range(-0.25f, 0.25f), 0.5f, Random.Range(0f, 0.95f));
                    }
                    item.Collider.isTrigger = true;
                } else
                {
                    inventory[i] = item;

                    item.transform.SetParent(handPosition);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localRotation = Quaternion.identity;

                    ikManager.UpdatePosition(ikHint);

                    item.Collider.isTrigger = true;
                    if (item.Rigidbody != null)
                    {
                        item.Rigidbody.isKinematic = true;
                    }
                }

                // Add item to hand?
                //item.gameObject.SetActive(false);
                pickupSound.Post(gameObject);

                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(Item item, int slot, float force)
    {
        inventory[slot] = null;
        ikManager.ResetPosition(ikHint.position);
        item.transform.parent = null;
        item.Collider.isTrigger = false;

        if (Physics.SphereCast(cam.position, 0.25f, cam.forward, out RaycastHit hit, interactRange, ~(1 << LayerMask.NameToLayer("Interactable"))))
        {
            if (hit.collider.CompareTag("ShoppingCart"))
            {
                if (item.Rigidbody != null)
                {
                    item.Rigidbody.isKinematic = true;
                    item.transform.parent = hit.transform;
                    item.transform.localPosition = new Vector3(Random.Range(-0.25f, 0.25f), 0.5f, Random.Range(0f, 0.95f));
                }
                item.Collider.isTrigger = true;
                return true;
            }
        }

        if (item.Rigidbody != null)
        {
            item.Rigidbody.isKinematic = false;
            item.Rigidbody.AddForce(cam.forward * force, ForceMode.Impulse);
        }
        dropSound.Post(gameObject);


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
