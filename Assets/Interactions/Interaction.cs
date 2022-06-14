using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAgentsFramework;

public abstract class Interaction : ScriptableObject
{
    public string flag;

    public string sentenceSource;
    public string sentenceTarget;

    // What the agent must be capable of in order to perform this interaction
    public List<Ability> requiredAbilities;

    // What the Smart Object must afford in order for the agent to perform this interaction on it
    public List<Affordance> requiredAffordances;

    public abstract void Perform(Agent agent, SmartObjectInstance smartObjectInstance);

    /*public void Check(Agent agent, SmartObject smartObject)
    {

    }*/
}
