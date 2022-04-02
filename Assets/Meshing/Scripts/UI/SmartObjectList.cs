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

    void OnEnable()
    {
        EventManager.OnSmartEnvironmentParsed += CreateObjectPanels;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDisable()
    {
        EventManager.OnSmartEnvironmentParsed -= CreateObjectPanels;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateObjectPanels(SmartObject[] smartObjects)
    {
        Debug.Log("UI: Entered smart object list creation!");
        foreach (SmartObject smartObject in smartObjects)
        {
            // Create a panel for the object
            GameObject panel = Instantiate(panelPrefab);
            // Attach the corresponding smart object (#TODO improve?)
            panel.GetComponent<SmartObjectListItem>().SetSmartObject(smartObject);
            // Adjust the icon on the panel
            panel.transform.GetChild(0).GetComponent<Image>().sprite = smartObject.objectIconUI;
            // Asjust the text label on the panel
            TextMeshProUGUI TMP_Text1 = panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI TMP_Text2 = panel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TMP_Text1.text = smartObject.nameSource;
            TMP_Text2.text = smartObject.nameTarget;
            // Adjust the instantiation button
            panel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { EventManager.SmartObjectParsed(smartObject); });
            // Place the panel
            panel.transform.SetParent(gameObject.transform, false);
            Debug.Log("UI: Smart object panel got placed!");
        }
    }
}
