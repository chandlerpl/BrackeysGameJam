using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class Footsteps : MonoBehaviour
{
    private Rigidbody cc;
    public AK.Wwise.Event _footstepSound;
    public AkEvent e;
    public void Step()
    {
        //Debug.Log("Tets");
        //_footstepSound.Post(gameObject);
        //e.HandleEvent(gameObject);
    }

/*    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {

        //if WASD pressed, then play audio source, if they're lifted, stop playing
        if ( Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            _audioSource.pitch = 0.5f;
            _audioSource.Play();
        }
        else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            _audioSource.Stop();
        }
        
       
        
        //if shift pressed, then play audio source, if they're lifted, stop playing
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _audioSource.pitch = 1f;
            _audioSource.Play();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _audioSource.Stop();
        }
        
    }*/
}
