using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointGroup : MonoBehaviour
{
    [SerializeField] private WaypointColour waypointColour = new WaypointColour();
    
    [SerializeField] private float stoppingDistance;

    [SerializeField] private List<Waypoint> waypoints = new List<Waypoint>();

    private void Awake()
    {
        waypoints = new List<Waypoint>();
        waypoints.AddRange(GetComponentsInChildren<Waypoint>());
    }

#if UNITY_EDITOR
    public bool showGizmosMode = false;

    public void ProjectOnTerrain()
    {
        RaycastHit hit;
        
        var ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit))
            Selection.activeTransform.position = hit.point;
        else
        {
            ray = new Ray(transform.position, transform.up);
            if (Physics.Raycast(ray, out hit))
                Selection.activeTransform.position = hit.point;
        }

        foreach (Waypoint waypoint in GetComponentsInChildren<Waypoint>())
        {
            waypoint.transform.position = new Vector3(waypoint.transform.position.x, transform.position.y, waypoint.transform.position.z);

            ray = new Ray(waypoint.transform.position, -transform.up);
            if (Physics.Raycast(ray, out hit))
                waypoint.transform.position = hit.point;
            else
            {
                ray = new Ray(waypoint.transform.position, transform.up);
                if (Physics.Raycast(ray, out hit))
                    waypoint.transform.position = hit.point;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmosMode)
            return;

        // Draw label.
        GUIStyle style = new GUIStyle();

        style.normal.textColor = waypointColour.OriginColor;

        Handles.BeginGUI();
        Vector3 pos = transform.position + transform.up * 0.5f;
        Vector2 pos2D = HandleUtility.WorldToGUIPoint(pos);
        GUI.Label(new Rect(pos2D.x + 10, pos2D.y - 10, 100, 100), name, style);
        Handles.EndGUI();

        Gizmos.color = waypointColour.OriginColor;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        var points = new List<Transform>();

        foreach (Waypoint point in GetComponentsInChildren<Waypoint>())
        {
            Handles.color = waypointColour.LineColor;
            Handles.DrawDottedLine(transform.position, point.transform.position, 1f);
            points.Add(point.transform);

            Handles.color = waypointColour.CircleColor;
            Handles.DrawWireDisc(point.transform.position, point.transform.up, 1f);
            Handles.DrawWireDisc(point.transform.position + Vector3.up * 2, point.transform.up, 1f);
        }

        if (points.Count <= 0) return;
        for (var i = 0; i < points.Count; i++)
        {
            Handles.color = waypointColour.CircleColor;
            var i_e = i + 1 == points.Count ? 0 : i + 1;
            Handles.DrawLine(points[i].position, points[i_e].position);
        }
    }
#endif

    public void SetWaypointsNumber(int number, float radius)
    {
        Clear();

        if (number == 0) return;

        for (int i = 0; i < number; i++)
        {
            var newWaypoint = new GameObject("Waypoint_" + i + "");
            newWaypoint.transform.position = transform.position;
            newWaypoint.transform.parent = transform;
            newWaypoint.AddComponent<Waypoint>();
        }

        SetWaypointsAtRadius(radius);
    }

    public void SetWaypointsAtRadius(float radius)
    {
        var number = 0;
        foreach (Transform child in transform)
            if (child.GetComponent<Waypoint>())
                number++;
        if (number == 0) return;
        var delta = 2 * Mathf.PI / number;
        var fi = 0f;
        foreach (Transform point in transform)
        {
            if (!point.GetComponent<Waypoint>()) continue;
            fi += delta;
            var x = radius * Mathf.Cos(fi);
            var z = radius * Mathf.Sin(fi);
            point.position = transform.position + new Vector3(x, 0, z);
        }
    }

    public void Clear()
    {
        foreach (Waypoint waypoint in GetComponentsInChildren<Waypoint>())
        {
            DestroyImmediate(waypoint.gameObject);
        }
    }

    public int WaypointsNumber()
    {
        return waypoints.Count;
    }

    public Waypoint GetWaypoint(int waypoint)
    {
        return waypoints[waypoint];
    }
}
