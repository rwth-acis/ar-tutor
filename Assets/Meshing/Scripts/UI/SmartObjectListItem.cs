using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// TextMeshPro
using TMPro;

public class SmartObjectListItem : MonoBehaviour
{
    public GameObject panel;
    public GameObject placementButton;
    public GameObject instructionLabel;
    private string virtualSOInstructionText = "1. Place the physical manifestation on a *** \n3. Place the affected area on the floor";
    private string physicalSOInstructionText = "1. Place the interactive area on a *** \n2. Place the affected area on the floor";

    // Instance ID (index) of a smart object attached to the button
    private int smartObjectInstanceIndex;

    void OnEnable()
    {
        EventManager.OnSmartObjectInstantiated += SmartObjectInstantiated;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDisable()
    {
        EventManager.OnSmartObjectInstantiated -= SmartObjectInstantiated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateSmartObject(int index)
    {
        // Attach the corresponding smart object's instance to the panel
        smartObjectInstanceIndex = index;

        var smartObjectInstance = SmartEnvironment.Instance.GetSmartObjectInstance(index);

        if (smartObjectInstance.smartObject.IsVirtual())
        {
            if (smartObjectInstance.smartObject.ContainsInteractiveArea())
            {
                instructionLabel.GetComponent<TextMeshProUGUI>().text = virtualSOInstructionText;
            }
        }
        else
        {
            instructionLabel.GetComponent<TextMeshProUGUI>().text = physicalSOInstructionText;
        }

        instructionLabel.GetComponent<TextMeshProUGUI>().text = instructionLabel.GetComponent<TextMeshProUGUI>().text.Replace("***", smartObjectInstance.smartObject.canBePlacedOn.ToString().ToLower());
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
