using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool announcementMade = false;
    
    public float announcementTiming = 60f;
    public AudioSource announcement;

    void Start()
    {
        instance = this;
        announcementMade = false;

        StartCoroutine(AnnouncementCountdown());
    }

    IEnumerator AnnouncementCountdown()
    {
        yield return new WaitForSeconds(announcementTiming);

        announcementMade = true;

        announcement.time = 5;
        announcement.Play();
    }
}
