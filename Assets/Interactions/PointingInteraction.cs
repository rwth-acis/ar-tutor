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
    }

    public override void Perform (Agent agent, SmartObject smartObject)
    {
        agent.PointTo(smartObject.interactiveArea);
        Debug.Log("Performed pointing interaction.");
    }
}
