using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StockingShelf : MonoBehaviour
{
    private static float MOVEMENT_TIME = 0.025f;
    [Tooltip("Time in seconds between court movement")]
    public Vector2 frequencyMinMax;
    [Tooltip("The position it should move to when blocking")]
    public Vector3 blockPosition = new Vector3(0, 0, 1);
    [Tooltip("The rotation it should move to when blocking")]
    public Vector3 blockRotation = new Vector3(0, 0, 0);

    private Vector3 _startPos;
    private Vector3 _endPos;
    private Quaternion _startRot;
    private Quaternion _endRot;

    private bool reverse = false;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _endPos = transform.position + blockPosition;

        _startRot = transform.rotation;
        _endRot = transform.rotation * Quaternion.Euler(blockRotation);

        StartCoroutine(MoveTrap());
    }

    IEnumerator MoveTrap()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(frequencyMinMax.x, frequencyMinMax.y));

            float time = 0;

            while(time < 1)
            {
                if(reverse)
                {
                    transform.position = Vector3.Lerp(_endPos, _startPos, time);
                    transform.rotation = Quaternion.Lerp(_endRot, _startRot, time);
                }
                else
                {
                    transform.position = Vector3.Lerp(_startPos, _endPos, time);
                    transform.rotation = Quaternion.Lerp(_startRot, _endRot, time);
                }

                yield return new WaitForSeconds(MOVEMENT_TIME);
                time += MOVEMENT_TIME;
            }

            reverse = !reverse;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(StockingShelf))]
public class StockingShelfEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    protected virtual void OnSceneGUI()
    {
        StockingShelf t = (StockingShelf)target;

        EditorGUI.BeginChangeCheck();
        Vector3 newTargetPosition = Handles.PositionHandle(t.transform.position + t.blockPosition, t.transform.rotation * Quaternion.Euler(t.blockRotation));
        Handles.Label(t.transform.position + t.blockPosition, "Move Position");

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(t, "Change Look At Target Position");
            t.blockPosition = newTargetPosition - t.transform.position;
        }
    }
}
#endif