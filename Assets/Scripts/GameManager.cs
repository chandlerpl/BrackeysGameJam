using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool announcementMade = false;

    [SerializeField] private ShoppingManager shoppingManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AudioManager audioManager;

    public float announcementTiming = 60f;
    public AK.Wwise.Event oneMinuteAnnouncement;
    public AK.Wwise.Event closedAnnouncement;

    public ShoppingManager ShoppingManager => shoppingManager;
    public GridManager GridManager => gridManager;
    public AudioManager AudioManager => audioManager;

    void Awake()
    {
        Instance = this;
        announcementMade = false;

        StartCoroutine(AnnouncementCountdown());
    }

    IEnumerator AnnouncementCountdown()
    {
        yield return new WaitForSeconds(2f);
        oneMinuteAnnouncement.Post(gameObject);

        yield return new WaitForSeconds(announcementTiming);

        announcementMade = true;
        closedAnnouncement.Post(gameObject);
    }
}
