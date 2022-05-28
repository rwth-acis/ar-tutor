using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Panel
using UnityEngine.UI;
// TextMeshPro
using TMPro;

public class SmartObjectList : MonoBehaviour
{
    [SerializeField] GameObject panelPrefab;
    private List<GameObject> panelList;

    void OnEnable()
    {
        //EventManager.OnSmartEnvironmentParsed += CreateObjectPanels;
        EventManager.OnSmartObjectParsed += CreateObjectPanel;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDisable()
    {
        //EventManager.OnSmartEnvironmentParsed -= CreateObjectPanels;
        EventManager.OnSmartObjectParsed -= CreateObjectPanel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void CreateObjectPanels(SmartObject[] smartObjects)
    {
        Debug.Log("UI: Entered smart object list creation!");
        foreach (SmartObject smartObject in smartObjects)
        {
            // Create a panel for the object
            GameObject panel = Instantiate(panelPrefab);
            // Adjust the icon on the panel
            panel.transform.GetChild(0).GetComponent<Image>().sprite = smartObject.objectIconUI;
            // Asjust the text label on the panel
            TextMeshProUGUI TMP_Text1 = panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI TMP_Text2 = panel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TMP_Text1.text = smartObject.nameSource;
            TMP_Text2.text = smartObject.nameTarget;
            // Adjust the instantiation button
            //TODO panel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { EventManager.InstantiateSmartObject(index); });
            // Place the panel
            panel.transform.SetParent(gameObject.transform, false);
            Debug.Log("UI: Smart object panel got placed!");
        }
    }*/

    void CreateObjectPanel(int index)
    {
        var smartObjectInstance = SmartEnvironment.Instance.GetSmartObjectInstance(index);
        // Create a panel for the object
        GameObject panel = Instantiate(panelPrefab);
        // Adjust the icon on the panel
        panel.transform.GetChild(0).GetComponent<Image>().sprite = smartObjectInstance.smartObject.objectIconUI;
        // Asjust the text label on the panel
        TextMeshProUGUI TMP_NameTarget = panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI TMP_NameSource = panel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TMP_NameTarget.text = "(" + smartObjectInstance.smartObject.nameTarget + ")";
        TMP_NameSource.text = "(" + smartObjectInstance.smartObject.nameSource + ")";
        // Adjust the instantiation button
        panel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { EventManager.InstantiateSmartObject(index); });
        panel.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += (" " + smartObjectInstance.smartObject.canBePlacedOn.ToString());
        Debug.Log("Set up object instantiation");
        // Place the panel
        panel.transform.SetParent(gameObject.transform, false);
        // Set the index of the smart object instance
        panel.GetComponent<SmartObjectListItem>().InstantiateSmartObject(index);
    }
}
