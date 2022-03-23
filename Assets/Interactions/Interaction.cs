using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAgentsFramework;

public abstract class Interaction : ScriptableObject
{
    public string flag;

    public string sentenceSource;
    public string sentenceTarget;

    // Agent's ability requirement
    public Ability requiredAbility;

    public string requiredAffordance;

    public abstract void Perform(Agent agent, SmartObject smartObject);

    /*public void Check(Agent agent, SmartObject smartObject)
    {

    }*/
}
