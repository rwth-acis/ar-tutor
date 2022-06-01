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
    [SerializeField] Interaction pointingInteraction;
    AgentAbilities abilities;
    //bool floorTracking;

    AgentTaskManager tempQueue;
    InstanceTransform tempAgentTransform;

    private void OnEnable()
    {
        // Register to event manager events
        EventManager.OnFloorInstantiated += FloorInstantiated;
        EventManager.OnAgentInstantiated += AgentInstantiated;
        //EventManager.OnWalkLabelInstantiated += WalkLabelInstantiated;
        EventManager.OnPointableSOInstantiated += PointableSOInstantiated;
        //EventManager.OnSmartObjectInstantiated += SmartObjectInstantiated;
    }

    private void OnDisable()
    {
        // Unregister from event manager events
        EventManager.OnFloorInstantiated -= FloorInstantiated;
        EventManager.OnAgentInstantiated -= AgentInstantiated;
        //EventManager.OnWalkLabelInstantiated -= WalkLabelInstantiated;
        EventManager.OnPointableSOInstantiated -= PointableSOInstantiated;
        //EventManager.OnSmartObjectInstantiated -= SmartObjectInstantiated;
    }

    private void Start()
    {
        agent = null;
        //floorTracking = false;
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
        //ScheduleInteractions();
    }

    // Called from event manager
    //TODO make work with SOIs
    /*private void WalkLabelInstantiated(SmartObject smartDestinationObject)
    {
        // WITH ABILITY SYSTEM
        // Make sure the NavMesh is built
        navMeshSurface.BuildNavMesh();
        // Try to perform a walk interaction
        Debug.Log("WalkLabel got instantiated.");
        InteractionManager.AttemptInteraction(agent, abilities, walkingInteraction, smartDestinationObject);

        WITHOUT ABILITY SYSTEM (commented out)
        // Queue walking to the destination for the agent
        if (agent != null)
        {
            agent.WalkTo(destination);
            //navMeshAgent.SetDestination(destination.transform.position);
            Debug.Log("WalkLabel got instantiated.");
        }
        
    }*/

    // Called from event manager
    private void ScheduleInteractions()
    {
        // Make sure the NavMesh is built
        navMeshSurface.BuildNavMesh();

        foreach (SmartObjectInstance smartObjectInstance in SmartEnvironment.Instance.GetSmartObjectInstances())
        {
            // Skip scheduling interactions for objects that have not been properly set up
            if (smartObjectInstance.instantiated == false)
                continue;
            // Try to perform a point interaction
            InteractionManager.AttemptInteraction(agent, abilities, pointingInteraction, smartObjectInstance);
        }
    }

    // Called from event manager
    public void PointableSOInstantiated(SmartObjectInstance pointableSmartObjectInstance)
    {
        // Make sure the NavMesh is built
        navMeshSurface.BuildNavMesh();
        // Try to perform a point interaction
        //TODO add object to interaction sequence planner?
		//InteractionManager.AttemptInteraction(agent, abilities, pointingInteraction, pointableSmartObjectInstance);
    }

    public void PlayAgentTasks()
    {
        ScheduleInteractions();
        tempQueue = agent.GetQueue();
        tempAgentTransform = new InstanceTransform(agent.gameObject.transform);
        agent.SetAgentState(Agent.State.idle);
    }

    public void StopAgentTasks()
    {
        if (!IsAgentSet())
            return;

        //agent.SetAgentState(Agent.State.inactive);
        agent.Deactivate();

        // Reset agent
        agent.SetQueue(tempQueue);
        // agent.Communicate("", "");
        tempAgentTransform.ApplyTransformTo(agent.gameObject.transform);

        //TODO Reposition the agent
        //agent.gameObject.transform.position = tempAgentTransform.position;
        //agent.gameObject.transform.rotation = tempAgentTransform.rotation;
        //agent.gameObject.transform.scale = tempAgentTransform.scale;
    }

    public bool IsAgentSet()
    {
        if (agent != null)
            return true;
        else
            return false;
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
