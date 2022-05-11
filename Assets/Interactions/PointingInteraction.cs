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
        base.sentenceTarget = "See on ***.";
    }

    public override void Perform (Agent agent, SmartObjectInstance pointableSmartObjectInstance)
    {
        //TODO Implement these as subinteractions or something or at least check the list of required affordances and abilities
        string temp1 = sentenceSource.Replace("***", pointableSmartObjectInstance.smartObject.nameSource);
        string temp2 = sentenceTarget.Replace("***", pointableSmartObjectInstance.smartObject.nameTarget);
        string sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(sentence);

        //TODO Implement these as subinteractions or something or at least check the list of required affordances and abilities
        // Walk to the marker
        agent.WalkTo(pointableSmartObjectInstance.affectedAreaGameObject);
        // Rotate towards the camera
        //GameObject camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        //agent.RotateTowards(camera.transform.position);
        // Rotate to be right from interactive area
        agent.RotateRelative(pointableSmartObjectInstance.interactiveAreaGameObject.transform.position, true);
        // Point
        Debug.Log("Current interactive area: " + pointableSmartObjectInstance.interactiveArea);
        agent.PointTo(pointableSmartObjectInstance.interactiveAreaGameObject);
        //agent.PlayAnimation("Pointing");

        agent.Communicate("");

        Debug.Log("Performed pointing interaction.");

    }
}
