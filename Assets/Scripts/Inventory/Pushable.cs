using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour, IInteractable
{
    public Transform pushableTransform;
    public Transform pushParent;

    private bool _isAttached = false;
    public void OnInteract(GameObject interactingObj, Inventory inventory)
    {
        if(!_isAttached)
        {
            pushableTransform.SetParent(pushParent);
            pushableTransform.localPosition = Vector3.zero;
            pushableTransform.localRotation = Quaternion.identity;
            _isAttached = true;
        } else
        {
            pushableTransform.SetParent(null);
            //pushableTransform.localPosition = Vector3.zero;
            _isAttached = false;
        }
    }
}
