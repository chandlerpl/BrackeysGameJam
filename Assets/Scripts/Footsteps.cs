using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class Footsteps : MonoBehaviour
{
    public AK.Wwise.Event _footstepSound;
    public AK.Wwise.Event _footstepRunSound;
    public AK.Wwise.Event _footstepBreathingSound;

    public float runDuration = 5f;

    private bool isRunning = false;
    private float startTime;
    public void Step()
    {
        _footstepSound.Post(gameObject);

        if(isRunning)
        {
            isRunning = false;

            if (startTime + runDuration < Time.time)
            {
                
            }
        }
    }
    public void StepRun()
    {
        _footstepRunSound.Post(gameObject);

        if(!isRunning)
        {
            isRunning = true;
            startTime = Time.time;
        }

        if(startTime + runDuration < Time.time)
        {
            _footstepBreathingSound.Post(gameObject);
        }
    }
}
