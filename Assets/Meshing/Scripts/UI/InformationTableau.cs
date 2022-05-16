using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TextMeshPro
using TMPro;

public class InformationTableau : MonoBehaviour
{
    public float seconds;
    GameObject panel;
    GameObject background;
    RectTransform backgroundRT;
    TextMeshProUGUI TMP_Text;
    string initialMessage = "First, place some objects from the menu on the right. Then point to the floor and place a virtual agent.";
    string tip;

    // Start is called before the first frame update
    void Start()
    {
        panel = this.gameObject;
        tip = "\n(Tap on this message to close it. Otherwise it will disappear after " + seconds + " seconds.)";
        background = panel.transform.GetChild(0).gameObject;
        backgroundRT = background.GetComponent<RectTransform>();
        TMP_Text = panel.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        PrintInformation(initialMessage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintInformation(string information)
    {
        StartCoroutine(PrintInformationCoroutine(information + tip, seconds));
    }

    IEnumerator PrintInformationCoroutine(string information, float seconds)
    {
        TMP_Text.text = information;
        Debug.Log("TMP value: " + TMP_Text.GetPreferredValues().ToString());
        //backgroundRT.sizeDelta = new Vector2(TMP_Text.GetPreferredValues().x + 10, backgroundRT.sizeDelta.y);
        Debug.Log("Printed information: " + information);
        background.SetActive(true);

        yield return new WaitForSeconds(seconds);

        background.SetActive(false);
    }
}
