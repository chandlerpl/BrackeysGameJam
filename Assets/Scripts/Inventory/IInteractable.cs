using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnInteract(GameObject interactingObj, PlayerInventory inventory);
}
