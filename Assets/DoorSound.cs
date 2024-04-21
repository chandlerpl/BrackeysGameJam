using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSound : MonoBehaviour, IInteractable
{
    public AudioSource source;

    public void OnInteract(GameObject interactingObj, Inventory inventory)
    {
        source.Play();

        if(GameManager.Instance.ShoppingManager != null && GameManager.Instance.ShoppingManager.HasAllRequiredItems(inventory))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene(2);
        }
    }
    
}
