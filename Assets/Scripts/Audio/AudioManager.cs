using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public List<AudioHear> audioListeners = new List<AudioHear>();
    public UnityEvent<AudioData> audioEvent;

    public void PlaySound(AudioData data)
    {
        audioEvent?.Invoke(data);
    }
}
