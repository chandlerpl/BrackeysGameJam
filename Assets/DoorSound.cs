using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSound : MonoBehaviour, IInteractable
{
    public void OnInteract(GameObject interactingObj, Inventory inventory)
    {
        if(GameManager.Instance.ShoppingManager != null && GameManager.Instance.ShoppingManager.HasCheckedOut)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnInteract(null, null);
    }
}
