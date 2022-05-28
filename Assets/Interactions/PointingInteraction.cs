using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAgentsFramework;

[CreateAssetMenu(fileName = "Pointing", menuName = "Interactions/Pointing")]
public class PointingInteraction : Interaction
{
    public string walkingSentenceSource = "I am walking towards ***.";
    public string walkingSentenceTarget = "Camino hacia ***.";
    //public string walkingSentenceTarget = "Kõnnin ***a/i poole.";

    public string rotatingSentenceSource = "I am turning towards ***.";
    public string rotatingSentenceTarget = "Me dirijo hacia ***.";

    public string pointingSentenceSource = "I am pointing at ***.";
    public string pointingSentenceTarget = "Señalo ***.";

    void OnEnable ()
    {
        base.flag = "Pointing";

        // "***" are placeholders for the smart object's name
        base.sentenceSource = "This is ***.";
        base.sentenceTarget = "Este/Esta es ***";
        //base.sentenceTarget = "See on ***.";
    }

    public override void Perform (Agent agent, SmartObjectInstance pointableSmartObjectInstance)
    {
        //TODO Implement these as subinteractions or something or at least check the list of required affordances and abilities
        string temp1 = walkingSentenceSource.Replace("***", pointableSmartObjectInstance.smartObject.nameSource);
        string temp2 = walkingSentenceTarget.Replace("***", pointableSmartObjectInstance.smartObject.nameTarget);
        string sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(sentence);
        // Additional wait
        agent.WaitForSeconds(1);
        //TODO Implement these as subinteractions or something or at least check the list of required affordances and abilities
        // Walk to the marker
        agent.WalkTo(pointableSmartObjectInstance.affectedAreaGameObject);
        // Rotate towards the camera
        //GameObject camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        //agent.RotateTowards(camera.transform.position);
        // Rotate to be right from interactive area
        //agent.RotateRelative(pointableSmartObjectInstance.interactiveAreaGameObject.transform.position, true);
        temp1 = rotatingSentenceSource.Replace("***", pointableSmartObjectInstance.smartObject.nameSource);
        temp2 = rotatingSentenceTarget.Replace("***", pointableSmartObjectInstance.smartObject.nameTarget);
        sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(sentence);

        agent.WaitForSeconds(1);

        agent.RotateTowards(pointableSmartObjectInstance.interactiveAreaGameObject.transform.position);

        temp1 = sentenceSource.Replace("***", pointableSmartObjectInstance.smartObject.nameSource);
        temp2 = sentenceTarget.Replace("***", pointableSmartObjectInstance.smartObject.nameTarget);
        sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(sentence);

        agent.WaitForSeconds(5);

        // Point
        temp1 = pointingSentenceSource.Replace("***", pointableSmartObjectInstance.smartObject.nameSource);
        temp2 = pointingSentenceTarget.Replace("***", pointableSmartObjectInstance.smartObject.nameTarget);
        sentence = temp2 + "\n" + "(" + temp1 + ")";
        agent.Communicate(sentence);

        agent.WaitForSeconds(1);

        Debug.Log("Current interactive area: " + pointableSmartObjectInstance.interactiveArea);
        agent.PointTo(pointableSmartObjectInstance.interactiveAreaGameObject);
        //agent.PlayAnimation("Pointing");

        agent.WaitForSeconds(1);

        agent.Communicate("");

        Debug.Log("Performed pointing interaction.");

    }
}
