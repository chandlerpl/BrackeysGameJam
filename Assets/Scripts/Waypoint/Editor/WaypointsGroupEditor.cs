#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor extention for WaypointsGroup objects.
/// </summary>
[CustomEditor(typeof(WaypointGroup))]
[CanEditMultipleObjects]
[Serializable]
public class WaypointsGroupEditor : Editor
{
	/// Specify the radius at which to set waypoints.
	[SerializeField] private float _radius = 1;
	/// Specify the number of waypoints to create.
	[SerializeField] private int _waypointsNum = 5;
	/// Wheather or not the user is currently placing waypoints.
	private bool _waypointsPlacementMode;
		
	/// <summary>
	/// This function is called when the object is loaded.
	/// </summary>
	private void OnEnable()
	{
		hideFlags = HideFlags.DontSave;
	}
		
	/// <summary>
	/// Draw the new inspector properties.
	/// </summary>
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
			
		EditorGUILayout.Space();
		EditorGUILayout.Space();
			
		serializedObject.Update();
			
		if (GUILayout.Button("Project On Plane"))
		{
            Selection.activeGameObject.GetComponent<WaypointGroup>().ProjectOnTerrain();
		}

		EditorGUILayout.Space();

		_waypointsPlacementMode = GUILayout.Toggle(_waypointsPlacementMode, "Place Waypoints", GUI.skin.button);
		serializedObject.ApplyModifiedProperties();
			
		EditorGUILayout.Space();
			
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Set Waypoints Number");
		_waypointsNum = EditorGUILayout.IntField(_waypointsNum);
		if (GUILayout.Button("Set Number"))
			Selection.activeGameObject.GetComponent<WaypointGroup>().SetWaypointsNumber(_waypointsNum, _radius);
		EditorGUILayout.EndHorizontal();
			
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Set On Radius");
		_radius = EditorGUILayout.FloatField(_radius);
		if (GUILayout.Button("Set"))
			Selection.activeGameObject.GetComponent<WaypointGroup>().SetWaypointsAtRadius(_radius);
		EditorGUILayout.EndHorizontal();
			
		EditorGUILayout.Space();
			
		if (GUILayout.Button("Clear Waypoints"))
			Selection.activeGameObject.GetComponent<WaypointGroup>().Clear();
			
	}
		
	/// <summary>
	/// Enables the Editor to handle an event in the scene view.
	/// </summary>
	private void OnSceneGUI()
	{
		if (_waypointsPlacementMode)
		{
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0
				&& !Event.current.command && !Event.current.control && !Event.current.alt)
			{
				var obj = Selection.activeObject;
				RaycastHit hit;
				var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				if (Physics.Raycast(ray, out hit))
				{
					var position = hit.point;

					var newWaypoint = new GameObject("Waypoint_" 
						                                + Selection.activeGameObject
							                                .GetComponent<WaypointGroup>().WaypointsNumber() + "");
					newWaypoint.transform.position = position;
					newWaypoint.transform.parent = Selection.activeTransform;
					newWaypoint.AddComponent<Waypoint>();
				}

				Selection.activeObject = obj;
				Event.current.Use();
			}
		}
	}
}
#endif