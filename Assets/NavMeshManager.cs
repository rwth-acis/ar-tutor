using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using VirtualAgentsFramework;
using VirtualAgentsFramework.AgentTasks;

public class NavMeshManager : MonoBehaviour
{
    Agent agent;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] Interaction walkingInteraction;
    AgentAbilities abilities;
    bool floorTracking;

    private void OnEnable()
    {
        // Register to event manager events
        EventManager.OnFloorInstantiated += FloorInstantiated;
        EventManager.OnAgentInstantiated += AgentInstantiated;
        EventManager.OnWalkLabelInstantiated += WalkLabelInstantiated;
    }

    private void OnDisable()
    {
        // Unregister from event manager events
        EventManager.OnFloorInstantiated -= FloorInstantiated;
        EventManager.OnAgentInstantiated -= AgentInstantiated;
        EventManager.OnWalkLabelInstantiated -= WalkLabelInstantiated;
    }

    private void Start()
    {
        floorTracking = false;
    }

    // Called from event manager
    private void FloorInstantiated()
    {
        // Start tracking the floor
        /*floorTracking = true;
        Debug.Log("Floor got instantiated.");*/
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh got rebuilt.");
    }

    // Called from event manager
    private void AgentInstantiated(Agent agent, AgentAbilities abilities)
    {
        // Start tracking the agent
        this.agent = agent;
        this.abilities = abilities;
        Debug.Log("Agent got instantiated.");
    }

    // Called from event manager
    private void WalkLabelInstantiated(SmartObject smartDestinationObject)
    {
        // WITH ABILITY SYSTEM
        // Try to perform a walk interaction
        InteractionManager.AttemptInteraction(agent, abilities, walkingInteraction, smartDestinationObject);

        /*  WITHOUT ABILITY SYSTEM
        // Queue walking to the destination for the agent
        if (agent != null)
        {
            agent.WalkTo(destination);
            //navMeshAgent.SetDestination(destination.transform.position);
            Debug.Log("WalkLabel got instantiated.");
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        // Update the NavMesh in every frame after the floor got instantiated (might be an overkill)
        /*if (floorTracking == true)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh got rebuilt.");
        }*/
    }
}
