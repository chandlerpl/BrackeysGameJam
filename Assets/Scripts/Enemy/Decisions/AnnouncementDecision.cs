using CP.AILibrary.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnnouncementDecision", menuName = "CP/Decision/Announcement")]
public class AnnouncementDecision : Decision
{
    public override bool Decide(StateMachine stateMachine)
    {
        return GameManager.announcementMade;
    }
}
