using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    public AK.Wwise.Event rollingSoundStart;
    public AK.Wwise.Event rollingSoundStop;

    private Rigidbody rb;
    private GridObject gridObject;
    private bool isPlaying;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gridObject = GetComponent<GridObject>();
    }

    private void FixedUpdate()
    {
        if(!isPlaying && rb.velocity.magnitude > 0.1f)
        {
            rollingSoundStart.Post(gameObject);
            GameManager.Instance.AudioManager.PlaySound(new AudioData()
            {
                position = rb.position,
                time = Time.time + 2f,
                range = 10f,
                priority = 2,
                gridObject = gridObject
            });
            isPlaying = true;
        } else if(isPlaying && rb.velocity.magnitude < 0.1f)
        {
            rollingSoundStop.Post(gameObject);
            isPlaying = false;
        }
    }

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
