using UnityEngine;
using UnityEditor;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
public class FindMissingScriptsRecursively : EditorWindow 
{
	static int go_count = 0, components_count = 0, missing_count = 0;

	[MenuItem("Window/FindMissingScriptsRecursively")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FindMissingScriptsRecursively));
	}

	public void OnGUI()
	{
		if (GUILayout.Button("Find Missing Scripts in selected GameObjects"))
		{
			FindInSelected();
		}
	}
	private static void FindInSelected()
	{
		GameObject[] go = Selection.gameObjects;
		go_count = 0;
		components_count = 0;
		missing_count = 0;
		foreach (GameObject g in go)
		{
			FindInGO(g);
		}
		Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
	}

	private static void FindInGO(GameObject g)
	{
		go_count++;
		List<Component> components = g.GetComponents<Component>().ToList();
		foreach(Component co in components)
		{
			components_count++;
			if (co == null)
			{
				missing_count++;
				string s = g.name;
				Transform t = g.transform;
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);
                while (t.parent != null) 
				{
					s = t.parent.name +"/"+s;
					t = t.parent;
				}
				Debug.Log (s + " has an empty script attached in position", g);
			}
		}
		// Now recurse through each child GO (if there are any):
		foreach (Transform childT in g.transform)
		{
			//Debug.Log("Searching " + childT.name  + " " );
			FindInGO(childT.gameObject);
		}
	}
}