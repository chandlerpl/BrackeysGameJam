using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //Add Behaviour change ability
    [SerializeField] private WaypointGroup changeWaypoints;

    public WaypointGroup ChangeWaypoints
    {
        get
        {
            return changeWaypoints;
        }
    }
}
