using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour, IInteractable
{
    public Transform pushableTransform;

    public List<IKHint> iKHints = new List<IKHint>();
    public bool isCart = false;

    private bool _isAttached = false;

    public void OnInteract(GameObject interactingObj, Inventory inventory)
    {
        if(!_isAttached)
        {/*
            pushableTransform.SetParent(pushParent);
            pushableTransform.localPosition = Vector3.zero;
            pushableTransform.localRotation = Quaternion.identity;*/

            pushableTransform.position = inventory.PushTransform.position;
            pushableTransform.localRotation = inventory.PushTransform.rotation;
            _isAttached = true;
            if (isCart)
                inventory.PushingCart = true;

            pushableTransform.gameObject.AddComponent<FixedJoint>().connectedBody = interactingObj.GetComponent<Rigidbody>();
            if (interactingObj.transform.GetChild(0).TryGetComponent(out IKManager manager))
            {
                foreach(IKHint ik in iKHints)
                {
                    manager.UpdatePosition(ik);
                }
            }
        } else
        {
            //pushableTransform.SetParent(null);
            //pushableTransform.localPosition = Vector3.zero;
            _isAttached = false;
            if (isCart)
                inventory.PushingCart = false;

            Destroy(pushableTransform.gameObject.GetComponent<FixedJoint>());
            if (interactingObj.transform.GetChild(0).TryGetComponent(out IKManager manager))
            {
                foreach (IKHint ik in iKHints)
                {
                    manager.ResetPosition(ik.position);
                }
            }
        }
    }
}
