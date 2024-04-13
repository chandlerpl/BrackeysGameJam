using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private float interactRange;

    [SerializeField]
    private Transform cam;

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

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
