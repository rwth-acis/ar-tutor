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
        EventManager.OnSmartObjectParsed += CreateObjectButton;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDisable()
    {
        EventManager.OnSmartEnvironmentParsed -= SmartEnvironmentParsed;
        EventManager.OnSmartObjectParsed -= CreateObjectButton;
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

    void CreateObjectButtons(SmartObject[] smartObjects)
    {
        Debug.Log("Entered wall button creation!");
        foreach(SmartObject smartObject in smartObjects)
        {
            CreateObjectButton(smartObject);
        }
    }

    void CreateObjectButton(SmartObject smartObject)
    {
        // Create a button for the object
        Button button = Instantiate(buttonPrefab); //, new Vector3(350,350,0), Quaternion.identity
                                                   // Add delegates with parameters to the button
        button.GetComponent<Button>().onClick.AddListener(delegate { InstantiatePhysicalManifestation(smartObject, button); });
        // Adjust the button's image
        smartObjectSprite = button.transform.GetChild(0).GetComponent<Image>().sprite;
        button.transform.GetChild(0).GetComponent<Image>().sprite = smartObject.objectIconUI;
        // Place the button on the right panel according to the classification
        switch (smartObject.canBePlacedOn)
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
                break;
        }
    }

    void InstantiatePhysicalManifestation(SmartObject smartObject, Button button)
    {
        Button buttonComponent = button.GetComponent<Button>();
        // Instantiate the embodiment
        classificationPlacementManager.PlaceWallObject(smartObject.physicalManifestation);
        // Adjust the button's image
        button.transform.GetChild(0).GetComponent<Image>().sprite = interactiveAreaSprite;
        // Add a different listener
        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener(delegate {InstantiateInteractiveArea(smartObject, buttonComponent); });
    }

    void InstantiateInteractiveArea(SmartObject smartObject, Button button)
    {
        Button buttonComponent = button.GetComponent<Button>();
        // Instantiate the interactive area
        GameObject interactiveArea = classificationPlacementManager.PlaceWallObject2(3);
        //TODO improve: add this object to the SmartObject collection in the scene
        smartObject.SetInteractiveArea(interactiveArea);
        // Move button to the floor objects UI
        button.transform.SetParent(floorObjectsUI.transform, false);
        // Adjust the button's image
        button.transform.GetChild(0).GetComponent<Image>().sprite = affectedAreaSprite;
        // Add a different listener
        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener(delegate {InstantiateAffectedArea(smartObject, buttonComponent); });
    }

    void InstantiateAffectedArea(SmartObject smartObject, Button button)
    {
        Button buttonComponent = button.GetComponent<Button>();
        // Instantiate the affected area
        GameObject affectedArea = classificationPlacementManager.PlaceFloorObject2(3);
        //TODO improve: add this object to the SmartObject collection in the scene
        smartObject.SetAffectedArea(affectedArea);
        // Schedule the tasks, TODO improve
        EventManager.PointableSOInstantiated(smartObject);
        Debug.Log("Smart object placement is finished!");
        // Disable the button
        button.gameObject.SetActive(false);
    }
}
