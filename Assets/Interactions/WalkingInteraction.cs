using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAgentsFramework;

[CreateAssetMenu(fileName = "Walking", menuName = "Interactions/Walking")]
public class WalkingInteraction : Interaction
{
    void OnEnable ()
    {
        base.flag = "Walking";
    }

    public override void Perform (Agent agent, SmartObjectInstance smartObjectInstance)
    {
        agent.WalkTo(smartObjectInstance.interactiveAreaGameObject);
    }
}
