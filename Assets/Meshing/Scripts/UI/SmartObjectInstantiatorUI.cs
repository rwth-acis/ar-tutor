using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Create and manage placement buttons for components of the Smart Objects.
/// </summary>
public class SmartObjectInstantiatorUI : MonoBehaviour
{
    [SerializeField] Button buttonPrefab;
    [SerializeField] GameObject wallObjectsUI;
    [SerializeField] GameObject floorObjectsUI;
    [SerializeField] GameObject tableObjectsUI;

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

    /// <summary>
    /// Create a placement button for the Smart Object instance.
    /// </summary>
	/// <param name="smartObjectInstance">Smart Object instance for which the button must be created.</param>
    public void CreateObjectButton(SmartObjectInstance smartObjectInstance)
    {
        // Create a button for the object
        Button button = Instantiate(buttonPrefab);
        // Attach the button to the SOI object
        smartObjectInstance.button = button.gameObject;
    }

    /// <summary>
    /// Adjust the placement button for the Smart Object instance.
    /// </summary>
	/// <param name="smartObjectInstance">Smart Object instance for which the button must be adjusted.</param>
	/// <param name="action">Action that will get triggered by pressing the button.</param>
	/// <param name="step">Step in the Smart Object instantiation process.</param>
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
                    break;
            }
        }
    }

    /// <summary>
    /// Set the button active or inactive.
    /// </summary>
	/// <param name="smartObjectInstance">Smart Object instance for which the button's state must be changed.</param>
	/// <param name="state">New state of the button.</param>
    public void ObjectButtonChangeState(SmartObjectInstance smartObjectInstance, bool state)
    {
        smartObjectInstance.button.SetActive(state);
    }
}