using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using VirtualAgentsFramework;
using VirtualAgentsFramework.AgentTasks;

public class NavMeshManager : MonoBehaviour
{
    Agent agent;
    NavMeshSurface floor;
    NavMeshAgent navmesh;

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

    // Called from event manager
    private void FloorInstantiated(NavMeshSurface floor)
    {
        // Start tracking the agent
        this.floor = floor;
        Debug.Log("Floor got instantiated.");
    }

    // Called from event manager
    private void AgentInstantiated(Agent agent, NavMeshAgent navmesh)
    {
        // Start tracking the agent
        this.agent = agent;
        this.navmesh = navmesh;
        Debug.Log("Agent got instantiated.");
    }

    // Called from event manager
    private void WalkLabelInstantiated(GameObject destination)
    {
        // Queue walking to the destination for the agent
        if (agent != null)
        {
            agent.WalkTo(gameObject);
            navmesh.SetDestination(gameObject.transform.position);
            Debug.Log("WalkLabel got instantiated.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the NavMesh in every frame (might be an overkill)
        if (floor != null)
        {
            floor.BuildNavMesh();
            Debug.Log("NavMesh got rebuilt.");
        }
    }
}
