using CP.AILibrary.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "EndAction", menuName = "CP/Action/End")]
public class EndAction : Action
{
    public override void Act(StateMachine stateMachine)
    {
        // GameOver stuff?
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(3);
    }

    public override void Enter(StateMachine stateMachine)
    {

    }

    public override void Exit(StateMachine stateMachine)
    {

    }
}
