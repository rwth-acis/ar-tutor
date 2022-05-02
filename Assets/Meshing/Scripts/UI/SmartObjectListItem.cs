using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartObjectListItem : MonoBehaviour
{
    public GameObject panel;
    public GameObject placementButton;
    public GameObject instructionLabel;

    // Instance ID (index) of a smart object attached to the button
    private int smartObjectInstanceIndex;

    void OnEnable()
    {
        //TODO This does not work with an event
        EventManager.OnInstantiateSmartObject += InstantiateSmartObject;
        EventManager.OnSmartObjectInstantiated += SmartObjectInstantiated;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDisable()
    {
        EventManager.OnInstantiateSmartObject -= InstantiateSmartObject;
        EventManager.OnSmartObjectInstantiated -= SmartObjectInstantiated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateSmartObject(int index)
    {
        // Attach the corresponding smart object's instance to the panel
        smartObjectInstanceIndex = index;
    }

    void SmartObjectInstantiated(int index)
    {
        if (this.smartObjectInstanceIndex != index)
            return;
        // Remove the instruction
        instructionLabel.SetActive(false);
        // Change panel color
        panel.GetComponent<Image>().color = new Color32(155, 255, 105, 100);

        // Adjust the button to enable smart object destruction
        //placementButton.GetComponent<Button>().onClick.{ RemoveAllListeners();
        //                                                 AddListener(delegate { RemoveSmartObjectInstance(smartObject); });
        //                                               };

        //placementButton.GetComponent<Button>().onClick.RemoveAllListeners();
        //placementButton.GetComponent<Button>().onClick.AddListener(delegate { RemoveSmartObjectInstance(smartObject); });
    }
}
