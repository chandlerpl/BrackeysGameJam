using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour, IInteractable
{
    public Transform pushableTransform;
    public Transform pushParent;

    public List<IKHint> iKHints = new List<IKHint>();

    private bool _isAttached = false;
    public void OnInteract(GameObject interactingObj, Inventory inventory)
    {
        if(!_isAttached)
        {
            pushableTransform.SetParent(pushParent);
            pushableTransform.localPosition = Vector3.zero;
            pushableTransform.localRotation = Quaternion.identity;
            _isAttached = true;

            if(pushParent.parent.TryGetComponent(out IKManager manager))
            {
                foreach(IKHint ik in iKHints)
                {
                    manager.UpdatePosition(ik);
                }
            }
        } else
        {
            pushableTransform.SetParent(null);
            //pushableTransform.localPosition = Vector3.zero;
            _isAttached = false;

            if (pushParent.parent.TryGetComponent(out IKManager manager))
            {
                foreach (IKHint ik in iKHints)
                {
                    manager.ResetPosition(ik.position);
                }
            }
        }
    }
}
