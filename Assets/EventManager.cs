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

    public delegate void AgentInstantiatedDelegate(Agent agent, NavMeshAgent navMeshAgent);

    public static event AgentInstantiatedDelegate OnAgentInstantiated;

    // Tells all the components when the agent got instantiated
    public static void AgentInstantiated(Agent agent, NavMeshAgent navMeshAgent)
    {
        OnAgentInstantiated?.Invoke(agent, navMeshAgent);
    }

    public delegate void WalkLabelInstantiatedDelegate(GameObject walkLabel);

    public static event WalkLabelInstantiatedDelegate OnWalkLabelInstantiated;

    // Tells all the components when the walk label got instantiated
    public static void WalkLabelInstantiated(GameObject walkLabel)
    {
        OnWalkLabelInstantiated?.Invoke(walkLabel);
    }
}
