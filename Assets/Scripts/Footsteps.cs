using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Footsteps : MonoBehaviour
{
    private AudioSource _audioSource;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude >= 1f && _audioSource.isPlaying == false)
        {
            _audioSource.pitch = Random.Range(0.0f, 1.1f);
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }
        // //if WASD pressed, then play audio source, if they're lifted, stop playing
        // if ( Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        // {
        //     _audioSource.pitch = 0.6f;
        //     _audioSource.Play();
        // }
        // else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        // {
        //     _audioSource.Stop();
        // }
        //
        //
        // //if shift pressed, then play audio source, if they're lifted, stop playing
        // if (Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     _audioSource.pitch = 0.8f;
        // }
        // else if (Input.GetKeyUp(KeyCode.LeftShift))
        // {
        //     _audioSource.Stop();
        // }
        
    }
}
