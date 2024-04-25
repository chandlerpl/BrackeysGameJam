using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public IKHint hint;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.GetComponent<IKManager>().UpdatePosition(hint);
    }
}
