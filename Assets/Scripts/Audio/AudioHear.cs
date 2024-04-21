using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHear : MonoBehaviour
{
    private Queue<AudioData> heardSounds = new Queue<AudioData>();

    public bool HasSoundQueued => heardSounds.Count > 0;
    void Start()
    {
        GameManager.Instance.AudioManager.audioEvent.AddListener(RegisterSound);
    }

    private void OnDisable()
    {
        GameManager.Instance.AudioManager.audioEvent.RemoveListener(RegisterSound);
    }

    private void OnDestroy()
    {
        GameManager.Instance.AudioManager.audioEvent.RemoveListener(RegisterSound);
    }

    public void RegisterSound(AudioData data)
    {
        heardSounds.Enqueue(data);
    }

    public AudioData Next()
    {
        return heardSounds.Dequeue();
    }
}

public class AudioData
{
    public Vector3 position;
    public float range;
    public int priority;
    public float time;
}
