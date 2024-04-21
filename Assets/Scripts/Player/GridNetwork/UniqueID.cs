#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// See https://docs.unity3d.com/ScriptReference/ExecuteInEditMode.html
[ExecuteAlways]
public class UniqueID : MonoBehaviour
{
    // Serialize/saves the unique ID
    // but hide it in the Inspector -> we don't want to edit it manually
    // uint allows around 4 Million IDs that should be enough for most cases ;)
    public uint _id;

    // Public read-only accessor
    public uint ID => _id;

#if UNITY_EDITOR
    // Due to ExecuteAllways this is called once the component is created
    private void Awake()
    {
        if (!Application.isPlaying)
        {
            if(_id == 0 || usedIDs.Contains(_id)) // Updated to support prefabs being added where a UniqueID has already been set from 0.
            {
                _id = GetFreeID();
                usedIDs.Add(_id);

                EditorUtility.SetDirty(this);
            }
        }
    }
#endif

    // For runtime e.g. when spawning prefabs
    // in that case Start is delayed so you can right after instantiating also assign a specific ID
    private void Start()
    {
        if (Application.isPlaying && _id == 0) _id = GetFreeID();
    }

    private void OnDestroy()
    {
        if(_id != 0)
        {
            usedIDs.Remove(_id);
        }
    }

    // Allows to set a specific ID e.g. when instantiating on runtime
    public void SetSpecificID(uint id)
    {
        if (Application.isPlaying) _id = id;
        else Debug.LogWarning("Only use in play mode!", this);
    }

    // Stores all already used IDs
    private readonly static HashSet<uint> usedIDs = new HashSet<uint>();

    private static readonly System.Random random = new System.Random();

    // Get a random uint
    private static uint RandomUInt()
    {
        uint thirtyBits = (uint)random.Next(1 << 30);
        uint twoBits = (uint)random.Next(1 << 2);
        return (thirtyBits << 2) | twoBits;
    }

    // This is called ONCE when the project is opened in the editor and ONCE when the app is started
    // See https://docs.unity3d.com/ScriptReference/InitializeOnLoadMethodAttribute.html
    [InitializeOnLoadMethod]
    [RuntimeInitializeOnLoadMethod]
    private static void InitUsedIDs()
    {
        // Find all instances of this class in the scene
        var instances = FindObjectsOfType<UniqueID>(true);
        foreach (var instance in instances.Where(i => i._id != 0))
        {
            if(usedIDs.Contains(instance._id))
            {
                // Something went wrong? Most likely a duplication or prefab!
                instance._id = GetFreeID();

#if UNITY_EDITOR
                // See https://docs.unity3d.com/ScriptReference/EditorUtility.SetDirty.html
                EditorUtility.SetDirty(instance);
#endif
            }
           
            usedIDs.Add(instance._id);
        }

        foreach (var instance in instances.Where(i => i._id == 0))
        {
            instance._id = GetFreeID();

#if UNITY_EDITOR
            // See https://docs.unity3d.com/ScriptReference/EditorUtility.SetDirty.html
            EditorUtility.SetDirty(instance);
#endif

            usedIDs.Add(instance._id);
        }
    }

    private static uint GetFreeID()
    {
        uint id = 0;

        do
        {
            id = RandomUInt();
        }
        while (id == 0 || usedIDs.Contains(id));

        return id;
    }
}