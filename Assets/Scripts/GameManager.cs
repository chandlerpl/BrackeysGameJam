using LSS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool announcementMade = false;

    public GameObject dayLights;
    public GameObject nightLights;

    public float announcementTiming = 60f;
    public AudioSource announcement;

    public string loadFolder = "NightLight-Data";

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

        dayLights.SetActive(false);
        nightLights.SetActive(true);
        GameObject.FindObjectOfType<LSS_FrontEnd>().Load(loadFolder);
    }
}
