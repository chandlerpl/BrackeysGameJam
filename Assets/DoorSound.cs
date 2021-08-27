using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSound : MonoBehaviour, IInteractable
{
    public AudioSource source;

    public void OnInteract(GameObject interactingObj, PlayerInventory inventory)
    {
        source.Play();
    }
    
}
