using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using VirtualAgentsFramework;
using VirtualAgentsFramework.AgentTasks;

/// <summary>
/// Manage the NavMesh navigation and hold an instance of the virtual agent. 
/// Schedule interaction attempts for the agent. 
/// </summary>
public class NavMeshManager : MonoBehaviour
{
    Agent agent;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] Interaction walkingInteraction;
    [SerializeField] Interaction pointingInteraction;
    AgentAbilities abilities;

    AgentTaskManager tempQueue;
    InstanceTransform tempAgentTransform;

    private void OnEnable()
    {
        // Register to event manager events. 
        EventManager.OnFloorInstantiated += FloorInstantiated;
        EventManager.OnAgentInstantiated += AgentInstantiated;
        EventManager.OnPointableSOInstantiated += PointableSOInstantiated;
        EventManager.OnDeactivateAgent += DeactivateAgent;
    }

    private void OnDisable()
    {
        // Unregister from event manager events. 
        EventManager.OnFloorInstantiated -= FloorInstantiated;
        EventManager.OnAgentInstantiated -= AgentInstantiated;
        EventManager.OnPointableSOInstantiated -= PointableSOInstantiated;
        EventManager.OnDeactivateAgent -= DeactivateAgent;
    }

    private void Start()
    {
        agent = null;
    }

    /// <summary>
    /// Start tracking the floor. 
	/// Called from event manager. 
    /// </summary>
    private void FloorInstantiated()
    {
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh got rebuilt.");
    }

    /// <summary>
    /// Start tracking the agent. 
	/// Called from event manager. 
    /// </summary>
	/// <param name="agent">Reference to the agent.</param>
	/// <param name="abilities">Reference to the agent's abilities.</param>
    private void AgentInstantiated(Agent agent, AgentAbilities abilities)
    {
        this.agent = agent.GetComponent<Agent>();
        this.abilities = abilities;
        Debug.Log("Agent got instantiated.");
    }

    /// <summary>
    /// Remove the agent instance. 
	/// Called from event manager.
    /// </summary>
    private void DeactivateAgent()
    {
        agent = null;
        abilities = null;
    }

    /// <summary>
    /// Rebuilds the NavMesh when a pointable Smart Object is instantiated. 
	/// Called from event manager. 
    /// </summary>
	/// <param name="pointableSmartObjectInstance">Reference to the pointable Smart Object instance.</param>
    public void PointableSOInstantiated(SmartObjectInstance pointableSmartObjectInstance)
    {
        // Make sure the NavMesh is built
        navMeshSurface.BuildNavMesh();
    }

    /// <summary>
    /// Start playing the agent tasks.
    /// </summary>
    public void PlayAgentTasks()
    {
        ScheduleInteractions();
        tempQueue = agent.GetQueue();
        tempAgentTransform = new InstanceTransform(agent.gameObject.transform);
        agent.SetAgentState(Agent.State.idle);
    }

    /// <summary>
    /// Create interaction attempts for the agent. 
	/// Helper method.
    /// </summary>
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

    /// <summary>
    /// Stop playing the agent tasks.
    /// </summary>
    public void StopAgentTasks()
    {
        if (!IsAgentSet())
            return;

        agent.Deactivate();

        // Reset agent
        agent.SetQueue(tempQueue);
        tempAgentTransform.ApplyTransformTo(agent.gameObject.transform);
    }

    /// <summary>
    /// Check whether the agent instance is set.
    /// </summary>
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

    }
}
