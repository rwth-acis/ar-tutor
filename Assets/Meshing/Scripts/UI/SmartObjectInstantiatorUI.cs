using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SmartObjectInstantiatorUI : MonoBehaviour
{
    [SerializeField] Button buttonPrefab;
    [SerializeField] GameObject wallObjectsUI;
    [SerializeField] GameObject floorObjectsUI;
    [SerializeField] GameObject tableObjectsUI;

    //SerializeField] Sprite smartObjectSprite;
    [SerializeField] Sprite interactiveAreaSprite;
    [SerializeField] Sprite affectedAreaSprite;

    SmartObject[] smartObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateObjectButton(SmartObjectInstance smartObjectInstance)
    {
        // Create a button for the object
        //TODO??? Was Button button previously
        Button button = Instantiate(buttonPrefab);
        // Attach the button to the SOI object
        smartObjectInstance.button = button.gameObject;
    }

    public void SetUpObjectButton(SmartObjectInstance smartObjectInstance, Action<SmartObjectInstance> action, int step)
    {
        var buttonComponent = smartObjectInstance.button.GetComponent<Button>();

        buttonComponent.onClick.RemoveAllListeners();

        buttonComponent.onClick.AddListener(delegate { action(smartObjectInstance); });
        if (step == 1)
            buttonComponent.transform.GetChild(0).GetComponent<Image>().sprite = smartObjectInstance.smartObject.objectIconUI;
        else if (step == 2)
            buttonComponent.transform.GetChild(0).GetComponent<Image>().sprite = interactiveAreaSprite;
        else if (step == 3)
            buttonComponent.transform.GetChild(0).GetComponent<Image>().sprite = affectedAreaSprite;

        // Place the button on the right panel according to the classification
        if (step == 3)
            smartObjectInstance.button.transform.SetParent(floorObjectsUI.transform, false);
        else
        {
            switch (smartObjectInstance.smartObject.canBePlacedOn)
            {
                case SmartObject.Placing.Wall:
                    smartObjectInstance.button.transform.SetParent(wallObjectsUI.transform, false);
                    Debug.Log("Wall button got placed!");
                    break;
                case SmartObject.Placing.Floor:
                    smartObjectInstance.button.transform.SetParent(floorObjectsUI.transform, false);
                    break;
                case SmartObject.Placing.Table:
                    smartObjectInstance.button.transform.SetParent(tableObjectsUI.transform, false);
                    break;
                default:
                    //TODO place on none/all in case of a physical object, add a boolean to check that?
                    break;
            }
        }
    }

    public void ObjectButtonChangeState(SmartObjectInstance smartObjectInstance, bool state)
    {
        smartObjectInstance.button.SetActive(state);
    }

    /*void InstantiatePhysicalManifestation(SmartObjectInstance smartObjectInstance, Button button)
    {
        Button buttonComponent = button.GetComponent<Button>();
        // Instantiate the embodiment
        GameObject physicalManifestation = classificationPlacementManager.PlaceWallObject(smartObjectInstance.smartObject.physicalManifestation);
        // Save the transform of the embodiment in the SO-instance object
        smartObjectInstance.physicalManifestation = new InstanceTransform(physicalManifestation.transform);
        // Adjust the button's image
        button.transform.GetChild(0).GetComponent<Image>().sprite = interactiveAreaSprite;
        // Add a different listener
        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener(delegate {InstantiateInteractiveArea(smartObjectInstance, buttonComponent); });
    }

    void InstantiateInteractiveArea(SmartObjectInstance smartObjectInstance, Button button)
    {
        // If interactiveArea is a part of physicalManifestation
        if (smartObjectInstance.smartObject.physicalManifestation == smartObjectInstance.smartObject.interactiveArea)
        {
            Debug.Log("interactiveArea is a part of physicalManifestation");
            // Fetch that object from the SmartAreas component
            smartObjectInstance.interactiveArea = new InstanceTransform(smartObjectInstance.smartObject.physicalManifestation.GetComponent<SmartAreas>().interactiveArea.transform);
        }
        else
        {
            Debug.Log("interactiveArea is NOT a part of physicalManifestation");
            // Instantiate the interactive area
            GameObject interactiveArea = classificationPlacementManager.PlaceWallObject2(3);
            //TODO improve: add this object to the SmartObject collection in the scene
            //smartObjectInstance.smartObject.SetInteractiveArea(interactiveArea);
            // Final scale as second argument
            smartObjectInstance.interactiveArea = new InstanceTransform(interactiveArea.transform, classificationPlacementManager.reticle.GetSmartAreaScale());
        }
        Button buttonComponent = button.GetComponent<Button>();
        // Move button to the floor objects UI
        button.transform.SetParent(floorObjectsUI.transform, false);
        // Adjust the button's image
        button.transform.GetChild(0).GetComponent<Image>().sprite = affectedAreaSprite;
        // Add a different listener
        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener(delegate {InstantiateAffectedArea(smartObjectInstance, buttonComponent); });
        Debug.Log("interactiveArea got instantiated");
    }

    void InstantiateAffectedArea(SmartObjectInstance smartObjectInstance, Button button)
    {
        Button buttonComponent = button.GetComponent<Button>();
        // Instantiate the affected area
        GameObject affectedArea = classificationPlacementManager.PlaceFloorObject2(3);
        //TODO improve: add this object to the SmartObject collection in the scene
        //smartObjectInstance.smartObject.SetAffectedArea(affectedArea);
        // Final scale as second argument
        smartObjectInstance.affectedArea = new InstanceTransform(affectedArea.transform, classificationPlacementManager.reticle.GetSmartAreaScale());

        // Schedule the tasks, TODO improve
        EventManager.PointableSOInstantiated(smartObjectInstance.smartObject);
        EventManager.SmartObjectInstantiated(SmartEnvironment.Instance.GetSmartObjectInstanceIndex(smartObjectInstance));
        Debug.Log("Smart object placement is finished!");
        // Disable the button
        button.gameObject.SetActive(false);
    }*/
}