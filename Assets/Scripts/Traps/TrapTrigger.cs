using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridObject))]
[RequireComponent(typeof(UniqueID))]
public class TrapTrigger : MonoBehaviour
{
    public List<Trap> traps = new List<Trap>();
    public List<GameObject> toggle = new ();
    public GameObject trapPrefab;

    private Collider col;
    private UniqueID uniqueID;

    public uint UniqueID => uniqueID.ID;

    private bool isEnabled;
    protected void Start()
    {
        uniqueID = GetComponent<UniqueID>();
        col = GetComponent<Collider>();

        GameManager.Instance.TrapManager.AddTrap(this);
    }

    public void Trigger()
    {
        if (isEnabled) return;
        isEnabled = true;

        foreach (Trap trap in traps)
        {
            trap.Enable();
        }

        foreach(GameObject go in toggle)
        {
            go.SetActive(true);
        }

        if(trapPrefab != null)
        {
            GameObject go = Instantiate(trapPrefab, transform);

            go.transform.localPosition = new Vector3(Random.Range(-6, 6), 0, 0);
        }
    }
}
