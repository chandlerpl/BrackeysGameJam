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

            if(audio.gridObject.Equals(data.GridObject))
            {
                continue;
            }

            if (Time.time > audio.time)
            {
                continue;
            }

            float sqrDist = (audio.position - data.transform.position).sqrMagnitude;
            if (sqrDist > audio.range * audio.range)
            {
                continue;
            }

            if (currData != null)
            {
                if(currData.priority > audio.priority)
                {
                    continue;
                }

                if (sqrDist > (currData.position - data.transform.position).sqrMagnitude)
                {
                    continue;
                }
            }

            currData = audio;
        }

        if(currData != null)
        {
            //Should investigate rather than run to specific place
            data.Agent.SetDestination(currData.position);

            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
