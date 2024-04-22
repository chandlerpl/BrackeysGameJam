using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDetectionNode : Node<AIMovement>
{
    public override bool Init(BehaviourTree<AIMovement> tree)
    {
        return true;
    }

    private AudioData currData;
    protected override NodeState Evaluate(AIMovement data)
    {
        if (currData != null && Time.time > currData.time)
        {
            currData = null;
        }

        while (data.AudioHear.HasSoundQueued)
        {
            AudioData audio = data.AudioHear.Next();

            if(audio.gridObject.Equals(data.GridObject)) // If it is the same object making sound then don't listen
            {
                continue;
            }
            if(audio.gridObject.type != GridObjectType.Player) // Temporarily only listen for sounds from the Player.
            {
                continue;
            }

            if (Time.time > audio.time) // A max time limit incase the event is unheard for a while.
            {
                continue;
            }

            float sqrDist = (audio.position - data.transform.position).sqrMagnitude;
            if (sqrDist > audio.range * audio.range) // Too far to hear!
            {
                continue;
            }

            if (currData != null) 
            {
                if(currData.priority > audio.priority) // The sound already being tracked was a bigger event
                {
                    continue;
                }

                if (sqrDist > (currData.position - data.transform.position).sqrMagnitude) // This sound is further3 than the previous sound
                {
                    continue;
                }
            }

            currData = audio;
            data.Agent.SetDestination(currData.gridObject.CurrentGrid.GetRandomPosition());
        }

        if(currData != null)
        {
            //Should investigate rather than run to specific place

            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
