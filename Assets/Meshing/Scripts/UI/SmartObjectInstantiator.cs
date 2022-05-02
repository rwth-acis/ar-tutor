using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartObjectInstantiator : MonoBehaviour
{
    [SerializeField] Button buttonPrefab;
    [SerializeField] GameObject wallObjectsUI;
    [SerializeField] GameObject floorObjectsUI;
    [SerializeField] GameObject tableObjectsUI;

    [SerializeField] ClassificationPlacementManager classificationPlacementManager;

    Sprite smartObjectSprite;
    [SerializeField] Sprite interactiveAreaSprite;
    [SerializeField] Sprite affectedAreaSprite;

    SmartObject[] smartObjects;

    void OnEnable()
    {
        EventManager.OnSmartEnvironmentParsed += SmartEnvironmentParsed;

        EventManager.OnSmartObjectParsed += CreateSmartObjectInstance;

        EventManager.OnInstantiateSmartObject += CreateObjectButton;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDisable()
    {
        EventManager.OnSmartEnvironmentParsed -= SmartEnvironmentParsed;

        EventManager.OnSmartObjectParsed -= CreateSmartObjectInstance;

        EventManager.OnInstantiateSmartObject -= CreateObjectButton;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Move this to a more persistent data model
    void SmartEnvironmentParsed(SmartObject[] smartObjects)
    {
        this.smartObjects = smartObjects;
    }

    /*void CreateObjectButtons(SmartObject[] smartObjects)
    {
        Debug.Log("Entered wall button creation!");
        foreach(SmartObject smartObject in smartObjects)
        {
            CreateObjectButton(smartObject);
        }
    }*/

    //TODO separate this to a UI script
    void CreateObjectButton(int index)
    {
        var smartObjectInstance = SmartEnvironment.Instance.GetSmartObjectInstance(index);
        // Create a button for the object
        Button button = Instantiate(buttonPrefab); //, new Vector3(350,350,0), Quaternion.identity
                                                   // Add delegates with parameters to the button
        if (smartObjectInstance.smartObject.physicalManifestation != null) // Only instantiate the physical manifestation for virtual objects
        {
            // Instantiate the physical manifestation
            button.GetComponent<Button>().onClick.AddListener(delegate { InstantiatePhysicalManifestation(smartObjectInstance, button); });
            // Adjust the button's image
            smartObjectSprite = button.transform.GetChild(0).GetComponent<Image>().sprite;
            button.transform.GetChild(0).GetComponent<Image>().sprite = smartObjectInstance.smartObject.objectIconUI;
        }
        else
        {
            // Instantiate the interactive area
            button.GetComponent<Button>().onClick.AddListener(delegate { InstantiateInteractiveArea(smartObjectInstance, button.GetComponent<Button>()); });
            // Adjust the button's image
            button.transform.GetChild(0).GetComponent<Image>().sprite = interactiveAreaSprite;
        }
        // Place the button on the right panel according to the classification
        switch (smartObjectInstance.smartObject.canBePlacedOn)
        {
            case SmartObject.Placing.Wall:
                button.transform.SetParent(wallObjectsUI.transform, false);
                Debug.Log("Wall button got placed!");
                break;
            case SmartObject.Placing.Floor:
                button.transform.SetParent(floorObjectsUI.transform, false);
                break;
            case SmartObject.Placing.Table:
                button.transform.SetParent(tableObjectsUI.transform, false);
                break;
            default:
                //TODO place on none/all in case of a physical object
                break;
        }
    }

    //TODO separate this to a logic script
    void CreateSmartObjectInstance(SmartObject smartObject)
    {
        int smartObjectIndex = SmartEnvironment.Instance.InsertSmartObject(new SmartObjectInstance(smartObject));
        EventManager.InstantiateSmartObject(smartObjectIndex);
    }

    void InstantiatePhysicalManifestation(SmartObjectInstance smartObjectInstance, Button button)
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
        Button buttonComponent = button.GetComponent<Button>();
        // Instantiate the interactive area
        GameObject interactiveArea = classificationPlacementManager.PlaceWallObject2(3);
        //TODO improve: add this object to the SmartObject collection in the scene
        smartObjectInstance.smartObject.SetInteractiveArea(interactiveArea);
        smartObjectInstance.interactiveArea = new InstanceTransform(interactiveArea.transform);

        // Move button to the floor objects UI
        button.transform.SetParent(floorObjectsUI.transform, false);
        // Adjust the button's image
        button.transform.GetChild(0).GetComponent<Image>().sprite = affectedAreaSprite;
        // Add a different listener
        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener(delegate {InstantiateAffectedArea(smartObjectInstance, buttonComponent); });
    }

    void InstantiateAffectedArea(SmartObjectInstance smartObjectInstance, Button button)
    {
        Button buttonComponent = button.GetComponent<Button>();
        // Instantiate the affected area
        GameObject affectedArea = classificationPlacementManager.PlaceFloorObject2(3);
        //TODO improve: add this object to the SmartObject collection in the scene
        smartObjectInstance.smartObject.SetAffectedArea(affectedArea);
        smartObjectInstance.affectedArea = new InstanceTransform(affectedArea.transform);

        // Schedule the tasks, TODO improve
        EventManager.PointableSOInstantiated(smartObjectInstance.smartObject);
        EventManager.SmartObjectInstantiated(SmartEnvironment.Instance.GetSmartObjectInstanceIndex(smartObjectInstance));
        Debug.Log("Smart object placement is finished!");
        // Disable the button
        button.gameObject.SetActive(false);
    }
}
