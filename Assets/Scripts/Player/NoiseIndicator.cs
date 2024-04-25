using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseIndicator : MonoBehaviour
{
    public Slider playerNoise;

    private float lastPlayerTime;
    void Start()
    {
        GameManager.Instance.AudioManager.audioEvent.AddListener(UpdateAudio);

        StartCoroutine(StartReset());
    }

    private void UpdateAudio(AudioData data)
    {
        if(data.gridObject.gameObject == gameObject)
        {
            playerNoise.value = data.priority;
            lastPlayerTime = Time.time;
        } else if(data.gridObject.type == GridObjectType.AIAgent)
        {

        }
    }

    public IEnumerator StartReset()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);

            if (Time.time > lastPlayerTime + .6f)
            {
                playerNoise.value = 0;
            }
        }
    }
}
