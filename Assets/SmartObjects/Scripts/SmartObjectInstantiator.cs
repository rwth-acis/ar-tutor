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
        EventManager.OnRestoreSmartObject += RestoreSmartObject;
    }

    void OnDisable()
    {
        EventManager.OnInstantiateSmartObject -= InstantiateSmartObject;
        EventManager.OnRestoreSmartObject -= RestoreSmartObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    void RestoreSmartObject(int index, Transform smartEnvironmentTransform)
    {
        var smartObjectInstance = SmartEnvironment.Instance.GetSmartObjectInstance(index);

        // Is this a virtual object?
        if (smartObjectInstance.smartObject.IsVirtual())
        {
            // Physical manifestation needs to be instantiated
            GameObject restoredPhysicalManifestation = Instantiate(smartObjectInstance.smartObject.physicalManifestation, smartEnvironmentTransform);
            smartObjectInstance.physicalManifestation.ApplyTransformTo(restoredPhysicalManifestation.transform);
            smartObjectInstance.physicalManifestationGameObject = restoredPhysicalManifestation;

            // Is interactive area the same as physical manifestation?
            if (smartObjectInstance.smartObject.ContainsInteractiveArea())
            {
                // Then the interactive area is a part of the physical manifestation -> fetch that object from the SmartAreas component
                smartObjectInstance.interactiveArea = new InstanceTransform(restoredPhysicalManifestation.GetComponent<SmartAreas>().interactiveArea.transform);
                smartObjectInstance.interactiveAreaGameObject = restoredPhysicalManifestation.GetComponent<SmartAreas>().interactiveArea;
                // Affected area still needs to be instantiated using smart areas
                GameObject restoredAffectedArea = Instantiate(smartObjectInstance.smartObject.affectedArea, smartEnvironmentTransform);
                smartObjectInstance.affectedArea.ApplyTransformTo(restoredAffectedArea.transform);
                smartObjectInstance.affectedAreaGameObject = restoredAffectedArea;
            }
            else
            {
                // Interactive and affected areas need to be instantiated using smart areas
                GameObject restoredInteractiveArea = Instantiate(smartObjectInstance.smartObject.interactiveArea, smartEnvironmentTransform);
                smartObjectInstance.interactiveArea.ApplyTransformTo(restoredInteractiveArea.transform);
                smartObjectInstance.interactiveAreaGameObject = restoredInteractiveArea;

                GameObject restoredAffectedArea = Instantiate(smartObjectInstance.smartObject.affectedArea, smartEnvironmentTransform);
                smartObjectInstance.affectedArea.ApplyTransformTo(restoredAffectedArea.transform);
                smartObjectInstance.affectedAreaGameObject = restoredAffectedArea;
            }
        }
        // Is this a real object?
        else
        {
            // Physical manifestation does not need to be instantiated
            // Interactive and affected areas need to be instantiated using smart areas
            GameObject restoredInteractiveArea = Instantiate(smartObjectInstance.smartObject.interactiveArea, smartEnvironmentTransform);
            smartObjectInstance.interactiveArea.ApplyTransformTo(restoredInteractiveArea.transform);
            smartObjectInstance.interactiveAreaGameObject = restoredInteractiveArea;

            GameObject restoredAffectedArea = Instantiate(smartObjectInstance.smartObject.affectedArea, smartEnvironmentTransform);
            smartObjectInstance.affectedArea.ApplyTransformTo(restoredAffectedArea.transform);
            smartObjectInstance.affectedAreaGameObject = restoredAffectedArea;
        }

        //TODO plan the behavioral sequence!!!
    }

    public void InstantiatePhysicalManifestation(SmartObjectInstance smartObjectInstance)
    {
        // Instantiate the physical manifestation
        GameObject physicalManifestation = classificationPlacementManager.PlaceWallObject(smartObjectInstance.smartObject.physicalManifestation);
        // Save the transform of the embodiment in the SO-instance object
        smartObjectInstance.physicalManifestation = new InstanceTransform(physicalManifestation.transform);
        smartObjectInstance.physicalManifestationGameObject = physicalManifestation;
        // Is interactive area the same as physical manifestation?
        if (smartObjectInstance.smartObject.physicalManifestation == smartObjectInstance.smartObject.interactiveArea)
        {
            // Then the physical manifestation contains the interactive area and has to be searched for it
            Debug.Log("interactiveArea is a part of physicalManifestation");
            // Fetch that object from the SmartAreas component
            //smartObjectInstance.interactiveArea = new InstanceTransform(smartObjectInstance.smartObject.physicalManifestation.GetComponent<SmartAreas>().interactiveArea.transform);
            smartObjectInstance.interactiveArea = new InstanceTransform(physicalManifestation.GetComponent<SmartAreas>().interactiveArea.transform);
            smartObjectInstance.interactiveAreaGameObject = physicalManifestation.GetComponent<SmartAreas>().interactiveArea;
            // Affected area still needs to be instantiated using smart areas
            // Set up the next step in UI
            smartObjectInstantiatorUI.SetUpObjectButton(smartObjectInstance, InstantiateAffectedArea, 3);
            EventManager.PostStatement("system", "instantiated", "interactive_area" + SmartEnvironment.Instance.GetSmartObjectInstanceIndex(smartObjectInstance).ToString());
        }
        else
        {
            // If no, the interactive area can be set up using smart areas
            // Set up the next step in UI
            smartObjectInstantiatorUI.SetUpObjectButton(smartObjectInstance, InstantiateInteractiveArea, 2);
        }
        EventManager.PostStatement("user", "instantiated", "physical_manifestation" + SmartEnvironment.Instance.GetSmartObjectInstanceIndex(smartObjectInstance).ToString());
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
        smartObjectInstance.interactiveAreaGameObject = interactiveArea;

        // Set up next step in UI
        smartObjectInstantiatorUI.SetUpObjectButton(smartObjectInstance, InstantiateAffectedArea, 3);

        Debug.Log("interactiveArea got instantiated");
        EventManager.PostStatement("user", "instantiated", "interactive_area" + SmartEnvironment.Instance.GetSmartObjectInstanceIndex(smartObjectInstance).ToString());
    }

    public void InstantiateAffectedArea(SmartObjectInstance smartObjectInstance)
    {
        // Instantiate the affected area
        GameObject affectedArea = classificationPlacementManager.PlaceFloorObject2(3);
        //TODO improve: add this object to the SmartObject collection in the scene
        //smartObjectInstance.smartObject.SetAffectedArea(affectedArea);
        // Final scale as second argument
        smartObjectInstance.affectedArea = new InstanceTransform(affectedArea.transform, classificationPlacementManager.reticle.GetSmartAreaScale());
        smartObjectInstance.affectedAreaGameObject = affectedArea;

        smartObjectInstance.instantiated = true;
        // Schedule the tasks, TODO improve
        EventManager.PointableSOInstantiated(smartObjectInstance);
        EventManager.SmartObjectInstantiated(SmartEnvironment.Instance.GetSmartObjectInstanceIndex(smartObjectInstance));
        EventManager.PostStatement("user", "instantiated", "affected_area" + SmartEnvironment.Instance.GetSmartObjectInstanceIndex(smartObjectInstance).ToString());

        // Set up next step in UI
        // Deactivate the button GameObject
        smartObjectInstantiatorUI.ObjectButtonChangeState(smartObjectInstance, false);

        Debug.Log("affectedArea got instantiated");
        Debug.Log("Smart object placement is finished!");
    }
}
