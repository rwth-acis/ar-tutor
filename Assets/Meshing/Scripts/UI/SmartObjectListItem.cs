using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartObjectListItem : MonoBehaviour
{
    public GameObject panel;
    public GameObject placementButton;
    public GameObject instructionLabel;

    // Smart object attacted to the button
    private SmartObject smartObject;

    void OnEnable()
    {
        //EventManager.OnSmartObjectParsed += SmartObjectParsed;
        EventManager.OnSmartObjectInstantiated += SmartObjectInstantiated;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDisable()
    {
        //EventManager.OnSmartObjectParsed -= SmartObjectParsed;
        EventManager.OnSmartObjectInstantiated -= SmartObjectInstantiated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSmartObject(SmartObject smartObject)
    {
        this.smartObject = smartObject;
    }

    void SmartObjectInstantiated(SmartObject smartObject)
    {
        if (this.smartObject != smartObject)
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
