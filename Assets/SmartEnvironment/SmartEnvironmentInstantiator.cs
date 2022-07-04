using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using VirtualAgentsFramework;

/// <summary>
/// Script that creates and manages a Smart Environment instance.
/// It also serves as a proxy for saving, loading, and resetting Smart Environments from the UI.
/// </summary>
public class SmartEnvironmentInstantiator : MonoBehaviour
{
    [SerializeField]
    private Transform smartEnvironmentTransform;

    [SerializeField]
    private Button agentPlacementButton;
    private Agent agent = null;
    private InstanceTransform agentTransform;

    private void OnEnable()
    {
        // Register to event manager events
        EventManager.OnSmartEnvironmentParsed += SmartEnvironmentParsed;
        EventManager.OnAgentInstantiated += AgentInstantiated;
    }

    private void OnDisable()
    {
        // Unregister from event manager events
        EventManager.OnSmartEnvironmentParsed -= SmartEnvironmentParsed;
        EventManager.OnAgentInstantiated -= AgentInstantiated;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AgentInstantiated(Agent agent, AgentAbilities abilities)
    {
        this.agent = agent;
        // Move the agent onto the SE transform
        agent.gameObject.transform.SetParent(smartEnvironmentTransform);
        // Save the agent's transform
        agentTransform = new InstanceTransform(agent.gameObject.transform);
    }

    public void Reset()
    {
        SmartEnvironment.Instance.Reset();

        if (agent != null)
        {
            DeactivateAgent();
            EventManager.DeactivateAgent();
        }
            
        RemoveObjectInstances();

        gameObject.GetComponent<SmartEnvironmentParser>().ParseSmartEnvironment();
    }

    public void Save()
    {
        SmartEnvironment.Instance.Save();
    }

    public void Load()
    {
        SmartEnvironment.Instance.Load();
    }

    public void Empty()
    {
        SmartEnvironment.Instance.EmptySE();
    }

    /// <summary>
    /// Remove all the GameObjects in the scene related to Smart Objects.
    /// </summary>
    public void RemoveObjectInstances()
    {
        foreach (Transform eachObject in smartEnvironmentTransform)
        {
            Destroy(eachObject.gameObject);
        }
        // Activate the agent placement button
        agentPlacementButton.interactable = true;
    }

    void DeactivateAgent()
    {
        agent.Deactivate();
    }

    /// <summary>
    /// Create Smart Object instances in the Smart Environment.
	/// Called from the EventManager.
    /// </summary>
    /// <param name="smartObjects">List of Smart Objects to be instantiated.</param>
    void SmartEnvironmentParsed(SmartObject[] smartObjects)
    {
        foreach (var smartObject in smartObjects)
        {
            int smartObjectIndex = SmartEnvironment.Instance.InsertSmartObject(new SmartObjectInstance(smartObject));
            EventManager.SmartObjectParsed(smartObjectIndex);
        }
    }

    /// <summary>
    /// Create GameObjects in the scene for every Smart Object instance from the Smart Environment.
	/// Called from the "Load" button.
    /// </summary>
    public void Instantiate()
    {
        SmartEnvironment.Instance.Load();

        // First, remove all the existing GameObjects
        RemoveObjectInstances();

        foreach (SmartObjectInstance smartObjectInstance in SmartEnvironment.Instance.GetSmartObjectInstances())
        {
            // Skip instantiating objects that have not been properly set up
            if (smartObjectInstance.instantiated == false)
                continue;

            EventManager.RestoreSmartObject(SmartEnvironment.Instance.GetSmartObjectInstanceIndex(smartObjectInstance), smartEnvironmentTransform);
            Debug.Log("Restored a smart object instance");
        }
    }
}
