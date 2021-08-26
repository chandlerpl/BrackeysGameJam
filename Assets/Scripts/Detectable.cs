using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityTypes
{
    Player,
    Alerted,

}

public class Detectable : MonoBehaviour
{
    public EntityTypes type;
    public float detectionTime = 0;
}
