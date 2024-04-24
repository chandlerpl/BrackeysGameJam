using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool announcementMade = false;

    [SerializeField] private ShoppingManager shoppingManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private TrapManager trapManager;
    [SerializeField] private AudioManager audioManager;

    public float announcementTiming = 60f;
    public AK.Wwise.Event oneMinuteAnnouncement;
    public AK.Wwise.Event closedAnnouncement;
    public Color closedLightsColour = new Color(0, 0.1122715f, 0.1471697f);
    public Light directionalLight;

    public ShoppingManager ShoppingManager => shoppingManager;
    public GridManager GridManager => gridManager;
    public AudioManager AudioManager => audioManager;
    public TrapManager TrapManager => trapManager;

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

        closedAnnouncement.Post(gameObject);
        yield return new WaitForSeconds(10f);

        float time = 0f;
        while(time < 1f)
        {
            time += 0.0125f;
            yield return new WaitForSeconds(0.05f);

            directionalLight.color = Color.Lerp(directionalLight.color, closedLightsColour, time);
        }

        announcementMade = true;
        directionalLight.color = closedLightsColour;
    }
}
