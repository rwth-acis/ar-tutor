using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObjectInstantiator : MonoBehaviour
{
    [SerializeField]
    ClassificationPlacementManager classificationPlacementManager;

    [SerializeField]
    SmartObjectInstantiatorUI smartObjectInstantiatorUI;

    void OnEnable()
    {
        EventManager.OnInstantiateSmartObject += InstantiateSmartObject;
    }

    void OnDisable()
    {
        EventManager.OnInstantiateSmartObject -= InstantiateSmartObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReviveSmartObject(int index)
    {

    }

    void InstantiateSmartObject(int index)
    {
        var smartObjectInstance = SmartEnvironment.Instance.GetSmartObjectInstance(index);

        smartObjectInstantiatorUI.CreateObjectButton(smartObjectInstance);

        // Is this going to be a virtual object?
        if (smartObjectInstance.smartObject.physicalManifestation != null)
        {
            // Physical manifestation needs to be instantiated
            smartObjectInstantiatorUI.SetUpObjectButton(smartObjectInstance, InstantiatePhysicalManifestation, 1);
            // Interactive and affected areas need to be instantiated using smart areas
        }
        // Is this going to be a real object?
        else
        {
            // Physical manifestation does not need to be instantiated
            // Interactive and affected areas need to be instantiated using smart areas
            smartObjectInstantiatorUI.SetUpObjectButton(smartObjectInstance, InstantiateInteractiveArea, 2);
        }  
    }

    public void InstantiatePhysicalManifestation(SmartObjectInstance smartObjectInstance)
    {
        // Instantiate the physical manifestation
        GameObject physicalManifestation = classificationPlacementManager.PlaceWallObject(smartObjectInstance.smartObject.physicalManifestation);
        // Save the transform of the embodiment in the SO-instance object
        smartObjectInstance.physicalManifestation = new InstanceTransform(physicalManifestation.transform);
        // Is interactive area the same as physical manifestation?
        if (smartObjectInstance.smartObject.physicalManifestation == smartObjectInstance.smartObject.interactiveArea)
        {
            // Then the physical manifestation contains the interactive area and has to be searched for it
            Debug.Log("interactiveArea is a part of physicalManifestation");
            // Fetch that object from the SmartAreas component
            smartObjectInstance.interactiveArea = new InstanceTransform(smartObjectInstance.smartObject.physicalManifestation.GetComponent<SmartAreas>().interactiveArea.transform);
            // Affected area still needs to be instantiated using smart areas
            // Set up the next step in UI
            smartObjectInstantiatorUI.SetUpObjectButton(smartObjectInstance, InstantiateAffectedArea, 3);
        }
        else
        {
            // If no, the interactive area can be set up using smart areas
            // Set up the next step in UI
            smartObjectInstantiatorUI.SetUpObjectButton(smartObjectInstance, InstantiateInteractiveArea, 2);
        }
    }

    public void InstantiateInteractiveArea(SmartObjectInstance smartObjectInstance)
    {
        // Instantiate the interactive area
        Debug.Log("interactiveArea is NOT a part of physicalManifestation");
        GameObject interactiveArea = classificationPlacementManager.PlaceWallObject2(3);
        //TODO improve: add this object to the SmartObject collection in the scene
        //smartObjectInstance.smartObject.SetInteractiveArea(interactiveArea);
        // Final scale as second argument
        smartObjectInstance.interactiveArea = new InstanceTransform(interactiveArea.transform, classificationPlacementManager.reticle.GetSmartAreaScale());

        // Set up next step in UI
        smartObjectInstantiatorUI.SetUpObjectButton(smartObjectInstance, InstantiateAffectedArea, 3);

        Debug.Log("interactiveArea got instantiated");
    }

    public void InstantiateAffectedArea(SmartObjectInstance smartObjectInstance)
    {
        // Instantiate the affected area
        GameObject affectedArea = classificationPlacementManager.PlaceFloorObject2(3);
        //TODO improve: add this object to the SmartObject collection in the scene
        //smartObjectInstance.smartObject.SetAffectedArea(affectedArea);
        // Final scale as second argument
        smartObjectInstance.affectedArea = new InstanceTransform(affectedArea.transform, classificationPlacementManager.reticle.GetSmartAreaScale());

        smartObjectInstance.instantiated = true;
        // Schedule the tasks, TODO improve
        EventManager.PointableSOInstantiated(smartObjectInstance.smartObject);
        EventManager.SmartObjectInstantiated(SmartEnvironment.Instance.GetSmartObjectInstanceIndex(smartObjectInstance));

        // Set up next step in UI
        // Deactivate the button GameObject
        smartObjectInstantiatorUI.ObjectButtonChangeState(smartObjectInstance, false);

        Debug.Log("affectedArea got instantiated");
        Debug.Log("Smart object placement is finished!");
    }
}
