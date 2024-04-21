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
        while(data.AudioHear.HasSoundQueued)
        {
            AudioData audio = data.AudioHear.Next();

            if(Time.time > audio.time)
            {
                continue;
            }
            if((audio.position - data.transform.position).sqrMagnitude > audio.range * audio.range)
            {
                continue;
            }

            if(currData != null)
            {
                if(currData.priority > audio.priority)
                {
                    continue;
                }
            }
        }

        return NodeState.Failure;
    }
}
