using UnityEngine.SceneManagement;
using UnityEngine;

public class CaughtNode : Node<AIMovement>
{
    public override bool Init(BehaviourTree<AIMovement> tree)
    {
        return true;
    }

    protected override NodeState Evaluate(AIMovement data)
    {
        // Caught the player! I need to do stuff here, but what?

        if(data.chasedPlayer != null && (data.transform.position - data.chasedPlayer.position).sqrMagnitude < 0.8f)
        {
/*            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(3);*/

            data.chasedPlayer.GetComponent<PlayerMovement>().RestartPlayer();
            data.GridsPlayerCaught.Add(data.GridObject.CurrentGrid.UniqueID);

            data.Agent.Warp(GameManager.Instance.GridManager.GetRandomGrid(data.GridsPlayerCaught).GetRandomPosition());
            // Time to remember what just happened!

            return NodeState.Success;
        }

        // Search for player?
        return NodeState.Running;
    }
}
