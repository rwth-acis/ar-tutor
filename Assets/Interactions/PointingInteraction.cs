using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAgentsFramework;

[CreateAssetMenu(fileName = "Pointing", menuName = "Interactions/Pointing")]
public class PointingInteraction : Interaction
{
    void OnEnable ()
    {
        base.flag = "Pointing";

        // "***" are placeholders for the smart object's name
        base.sentenceSource = "This is ***."; 
        base.sentenceTarget = "See ***.";
    }

    public override void Perform (Agent agent, SmartObject smartObject)
    {
        string temp1 = sentenceSource.Replace("***", smartObject.nameSource);
        string temp2 = sentenceTarget.Replace("***", smartObject.nameTarget);
        string sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(sentence);
        agent.PointTo(smartObject.interactiveArea);
        Debug.Log("Performed pointing interaction.");
    }
}
