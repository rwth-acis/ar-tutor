using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Panel
using UnityEngine.UI;
// TextMeshPro
using TMPro;

/// <summary>
/// Represent the list of UI panels corresponding to Smart Object instances.
/// </summary>
public class SmartObjectList : MonoBehaviour
{
    [SerializeField] GameObject panelPrefab;
    private List<GameObject> panelList;

    void OnEnable()
    {
        EventManager.OnSmartObjectParsed += CreateObjectPanel;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDisable()
    {
        EventManager.OnSmartObjectParsed -= CreateObjectPanel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates a UI panel for a Smart Object instance.
    /// </summary>
    /// <param name="index">Index of the Smart Object instance for thispanel.</param>
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
        TMP_NameTarget.text = smartObjectInstance.smartObject.nameTarget;
        TMP_NameSource.text = "(" + smartObjectInstance.smartObject.nameSource + ")";
        // Adjust the instantiation button
        panel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { EventManager.InstantiateSmartObject(index); });
        panel.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += (" " + smartObjectInstance.smartObject.canBePlacedOn.ToString().ToLower());
        Debug.Log("Set up object instantiation");
        // Place the panel
        panel.transform.SetParent(gameObject.transform, false);
        // Set the index of the smart object instance
        panel.GetComponent<SmartObjectListItem>().InstantiateSmartObject(index);
    }

    /// <summary>
    /// Remove all panels corresponding to Smart Object instances.
    /// </summary>
    public void RemoveObjectPanels()
    {
        foreach (Transform eachObject in gameObject.transform)
        {
            Destroy(eachObject.gameObject);
        }
    }
}
