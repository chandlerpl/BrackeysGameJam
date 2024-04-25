using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour, IInteractable
{
    public Transform pushableTransform;

    public List<IKHint> iKHints = new List<IKHint>();
    public bool isCart = false;

    private bool _isAttached = false;
    private IKManager _currentIK;

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

            inventory.CurrentPushable = this;
            pushableTransform.gameObject.AddComponent<FixedJoint>().connectedBody = interactingObj.GetComponent<Rigidbody>();
            if (interactingObj.transform.GetChild(0).TryGetComponent(out _currentIK))
            {
                foreach(IKHint ik in iKHints)
                {
                    _currentIK.UpdatePosition(ik);
                }
            }
        } else
        {
            Detach(inventory);
        }
    }

    public void Detach(Inventory inventory)
    {
        if (!_isAttached) return;

        _isAttached = false;
        if (isCart)
            inventory.PushingCart = false;
        inventory.CurrentPushable = null;

        Destroy(pushableTransform.gameObject.GetComponent<FixedJoint>());
        foreach (IKHint ik in iKHints)
        {
            _currentIK.ResetPosition(ik.position);
        }
        _currentIK = null;
    }
}
