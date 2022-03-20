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
    [SerializeField] GameObject camera;
    AgentAbilities abilities;
    bool floorTracking;

    private void OnEnable()
    {
        // Register to event manager events
        EventManager.OnFloorInstantiated += FloorInstantiated;
        EventManager.OnAgentInstantiated += AgentInstantiated;
        EventManager.OnWalkLabelInstantiated += WalkLabelInstantiated;
        EventManager.OnPointableSOInstantiated += PointableSOInstantiated;
    }

    private void OnDisable()
    {
        // Unregister from event manager events
        EventManager.OnFloorInstantiated -= FloorInstantiated;
        EventManager.OnAgentInstantiated -= AgentInstantiated;
        EventManager.OnWalkLabelInstantiated -= WalkLabelInstantiated;
        EventManager.OnPointableSOInstantiated -= PointableSOInstantiated;
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
        this.agent = agent.GetComponent<Agent>();
        this.abilities = abilities;
        Debug.Log("Agent got instantiated.");

        //Animation test
        //agent.PlayAnimation("Dancing");
        //Debug.Log("Performed dancing animation.");
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

    // Called from event manager
    private void PointableSOInstantiated(SmartObject pointableSmartObject)
    {
        //TODO implement this in terms of Interactions
        navMeshSurface.BuildNavMesh();
        if (agent == null)
            return;
        agent.WalkTo(pointableSmartObject.affectedArea);
        // Rotate towards the camera
        agent.RotateTowards(camera.transform.position);
        Debug.Log("Current interactive area: " + pointableSmartObject.interactiveArea);
        agent.PointTo(pointableSmartObject.interactiveArea);
        //agent.PlayAnimation("Pointing");
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
