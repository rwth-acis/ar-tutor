using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using VirtualAgentsFramework;

public class EventManager : MonoBehaviour
{
    public delegate void FloorInstantiatedDelegate();

    public static event FloorInstantiatedDelegate OnFloorInstantiated;

    // Tells all the components when the floor got instantiated
    public static void FloorInstantiated()
    {
        OnFloorInstantiated?.Invoke();
    }

    public delegate void AgentInstantiatedDelegate(Agent agent, AgentAbilities abilities);

    public static event AgentInstantiatedDelegate OnAgentInstantiated;

    // Tells all the components when the agent got instantiated
    public static void AgentInstantiated(Agent agent, AgentAbilities abilities)
    {
        OnAgentInstantiated?.Invoke(agent, abilities);
    }

    public delegate void WalkLabelInstantiatedDelegate(SmartObject smartDestinationObject);

    public static event WalkLabelInstantiatedDelegate OnWalkLabelInstantiated;

    // Tells all the components when the walk label got instantiated
    public static void WalkLabelInstantiated(SmartObject smartDestinationObject)
    {
        OnWalkLabelInstantiated?.Invoke(smartDestinationObject);
    }

    // ***SmartObject events***

    public delegate void SmartObjectParsedDelegate(SmartObject smartObject);

    public static event SmartObjectParsedDelegate OnSmartObjectParsed;

    // Tells all the components that a SmartObject has been parsed and can be instantiated
    public static void SmartObjectParsed(SmartObject smartObject)
    {
        OnSmartObjectParsed?.Invoke(smartObject);
    }

    //TODO make this method universal
    public delegate void PointableSOInstantiatedDelegate(SmartObject pointableSO);

    public static event PointableSOInstantiatedDelegate OnPointableSOInstantiated;

    // Tells all the components when a pointable Smart Object got instantiated
    public static void PointableSOInstantiated(SmartObject pointableSO)
    {
        OnPointableSOInstantiated?.Invoke(pointableSO);
    }

}
