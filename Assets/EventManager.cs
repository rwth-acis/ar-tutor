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

    // ***SmartEnvironment events***

    public delegate void SmartEnvironmentParsedDelegate(SmartObject[] smartObjects);

    public static event SmartEnvironmentParsedDelegate OnSmartEnvironmentParsed;

    // Tells all the components that a SmartEnvironment has been parsed and can be instantiated
    public static void SmartEnvironmentParsed(SmartObject[] smartObjects)
    {
        OnSmartEnvironmentParsed?.Invoke(smartObjects);
    }

    // ***SmartObject events***

    public delegate void SmartObjectParsedDelegate(int index);

    public static event SmartObjectParsedDelegate OnSmartObjectParsed;

    // Tells all the components that a SmartObject has been parsed (an instance object was created) and can be instantiated
    public static void SmartObjectParsed(int index)
    {
        OnSmartObjectParsed?.Invoke(index);
    }

    //TODO make this method universal
    public delegate void PointableSOInstantiatedDelegate(SmartObjectInstance pointableSOI);

    public static event PointableSOInstantiatedDelegate OnPointableSOInstantiated;

    // Tells all the components when a pointable Smart Object got instantiated
    public static void PointableSOInstantiated(SmartObjectInstance pointableSOI)
    {
        OnPointableSOInstantiated?.Invoke(pointableSOI);
    }


    public delegate void InstantiateSmartObjectDelegate(int index);

    public static event InstantiateSmartObjectDelegate OnInstantiateSmartObject;

    // Tells all the relevant components that a SmartObject instance with index "index" has started the instantiation process
    public static void InstantiateSmartObject(int index)
    {
        OnInstantiateSmartObject?.Invoke(index);
    }


    public delegate void SmartObjectInstantiatedDelegate(int index);

    public static event SmartObjectInstantiatedDelegate OnSmartObjectInstantiated;

    // Tells all the components that a SmartObject with index "index" has been completely instantiated and is now placed in the scene
    public static void SmartObjectInstantiated(int index)
    {
        OnSmartObjectInstantiated?.Invoke(index);
    }

    public delegate void RestoreSmartObjectDelegate(int index, Transform smartEnvironmentTransform);

    public static event RestoreSmartObjectDelegate OnRestoreSmartObject;

    // Tells all the relevant components that a SmartObject instance with index "index" should be retrieved
    public static void RestoreSmartObject(int index, Transform smartEnvironmentTransform)
    {
        OnRestoreSmartObject?.Invoke(index, smartEnvironmentTransform);
    }

    public delegate void PostStatementDelegate(string agent, string verb, string obj);

    public static event PostStatementDelegate OnPostStatement;

    // Tells the xAPI module to post a statement
    public static void PostStatement(string agent, string verb, string obj)
    {
        OnPostStatement?.Invoke(agent, verb, obj);
    }

    public delegate void SXStatusChangedDelegate(bool play);

    public static event SXStatusChangedDelegate OnSXStatusChanged;

    // Tells all the components that the status of the smart experience (SX) has changed allow playing, iff play == true
    public static void SXStatusChanged(bool play)
    {
        OnSXStatusChanged?.Invoke(play);
    }
}
