using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using VirtualAgentsFramework;

public class SmartEnvironmentInstantiator : MonoBehaviour
{
    [SerializeField]
    private Transform smartEnvironmentTransform;

    [SerializeField]
    private Button agentPlacementButton;
    //private GameObject agentPrefab;
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
        // Move the agent onto the SE transform
        agent.gameObject.transform.SetParent(smartEnvironmentTransform);
        // Save the agent's transform
        agentTransform = new InstanceTransform(agent.gameObject.transform);
    }

    public void RemoveObjectInstances()
    {
        foreach (Transform eachObject in smartEnvironmentTransform)
        {
            Destroy(eachObject.gameObject);
            //eachObject.gameObject.SetActive(false);
        }
        // Activate the agent placement button
        agentPlacementButton.interactable = true;
    }

    //TODO rename: instantiate smart environment or create so instances
    void SmartEnvironmentParsed(SmartObject[] smartObjects)
    {
        foreach (var smartObject in smartObjects)
        {
            int smartObjectIndex = SmartEnvironment.Instance.InsertSmartObject(new SmartObjectInstance(smartObject));
            EventManager.SmartObjectParsed(smartObjectIndex);
        }
    }

    public void Instantiate()
    {
        // First, remove all the existing GameObjects
        RemoveObjectInstances();

        //TODO rename "smartEnvironment" to "objectCollection"
        //TODO move the activation method to a SmartObjectInstanceTools class
        //foreach (SmartObjectInstance smartObjectInstance in SmartEnvironment.Instance.smartEnvironment)
        foreach (SmartObjectInstance smartObjectInstance in SmartEnvironment.Instance.GetSmartObjectInstances())
        {
            // Skip instantiating objects that have not been properly set up
            if (smartObjectInstance.instantiated == false)
                continue;

            GameObject restoredPhysicalManifestation = Instantiate(smartObjectInstance.smartObject.physicalManifestation, smartEnvironmentTransform);
            smartObjectInstance.physicalManifestation.ApplyTransformTo(restoredPhysicalManifestation.transform);

            GameObject restoredInteractiveArea = Instantiate(smartObjectInstance.smartObject.interactiveArea, smartEnvironmentTransform);
            smartObjectInstance.interactiveArea.ApplyTransformTo(restoredInteractiveArea.transform);

            GameObject restoredAffectedArea = Instantiate(smartObjectInstance.smartObject.affectedArea, smartEnvironmentTransform);
            smartObjectInstance.affectedArea.ApplyTransformTo(restoredAffectedArea.transform);
            /*smartObjectInstance.physicalManifestation.transform.parent = smartEnvironmentTransform;
            smartObjectInstance.physicalManifestation.SetActive(true);
            //Instantiate(smartObjectInstance.interactiveArea, smartEnvironmentTransform);
            smartObjectInstance.interactiveArea.transform.parent = smartEnvironmentTransform;
            smartObjectInstance.interactiveArea.SetActive(true);
            //Instantiate(smartObjectInstance.affectedArea, smartEnvironmentTransform);
            smartObjectInstance.affectedArea.transform.parent = smartEnvironmentTransform;
            smartObjectInstance.affectedArea.SetActive(true);*/
            Debug.Log("Restored a smart object instance");
        }
    }
}
