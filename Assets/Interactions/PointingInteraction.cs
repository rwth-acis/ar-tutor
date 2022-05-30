using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAgentsFramework;

[CreateAssetMenu(fileName = "Pointing", menuName = "Interactions/Pointing")]
public class PointingInteraction : Interaction
{
    public string walkingSentenceSource = "I am walking towards ***.";
    public string walkingSentenceTarget = "Kõnnin *** poole."; // maali, kella, äratuskella, laualambi

    public string rotatingSentenceSource = "I am turning towards ***.";
    public string rotatingSentenceTarget = "Keeran *** poole."; // maali, kella, äratuskella, laualambi

    public string pointingSentenceSource = "I am pointing at ***.";
    public string pointingSentenceTarget = "Osutan ***."; // maalile, kellale, äratuskellale, laualambile

    void OnEnable ()
    {
        base.flag = "Pointing";

        // "***" are placeholders for the smart object's name
        base.sentenceSource = "In front of me is ***.";
        base.sentenceTarget = "Minu ees on ***"; // maal, kell, äratuskell, laualamp
    }

    public override void Perform (Agent agent, SmartObjectInstance pointableSmartObjectInstance)
    {
        //TODO Implement these as subinteractions or something or at least check the list of required affordances and abilities
        string temp1 = walkingSentenceTarget.Replace("***", pointableSmartObjectInstance.smartObject.nameTarget_TowardsCase);
        string temp2 = walkingSentenceSource.Replace("***", pointableSmartObjectInstance.smartObject.nameSource);
        //string sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(temp1, temp2, false);

        // Additional wait
        //agent.WaitForSeconds(1);

        //TODO Implement these as subinteractions or something or at least check the list of required affordances and abilities
        // Walk to the marker
        agent.WalkTo(pointableSmartObjectInstance.affectedAreaGameObject);
        agent.Communicate(temp1, temp2, true);
        // Rotate towards the camera
        //GameObject camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        //agent.RotateTowards(camera.transform.position);
        // Rotate to be right from interactive area
        //agent.RotateRelative(pointableSmartObjectInstance.interactiveAreaGameObject.transform.position, true);
        temp1 = rotatingSentenceTarget.Replace("***", pointableSmartObjectInstance.smartObject.nameTarget_TowardsCase);
        temp2 = rotatingSentenceSource.Replace("***", pointableSmartObjectInstance.smartObject.nameSource);
        //sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(temp1, temp2, false);

        // Additional wait
        //agent.WaitForSeconds(1);

        agent.RotateTowards(pointableSmartObjectInstance.interactiveAreaGameObject.transform.position);
        agent.Communicate(temp1, temp2, true);

        // Standing in front
        temp1 = sentenceTarget.Replace("***", pointableSmartObjectInstance.smartObject.nameTarget);
        temp2 = sentenceSource.Replace("***", pointableSmartObjectInstance.smartObject.nameSource);
        //sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(temp1, temp2, true);

        // Additional wait
        //agent.WaitForSeconds(5);

        // Point
        temp1 = pointingSentenceTarget.Replace("***", pointableSmartObjectInstance.smartObject.nameTarget_AtCase);
        temp2 = pointingSentenceSource.Replace("***", pointableSmartObjectInstance.smartObject.nameSource);
        //sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(temp1, temp2, false);

        // Additional wait
        //agent.WaitForSeconds(1);

        Debug.Log("Current interactive area: " + pointableSmartObjectInstance.interactiveArea);
        agent.PointTo(pointableSmartObjectInstance.interactiveAreaGameObject);
        //agent.PlayAnimation("Pointing");

        //agent.WaitForSeconds(1);

        agent.Communicate(temp1, temp2, true);
        //agent.Communicate("");

        Debug.Log("Performed pointing interaction.");

    }
}
