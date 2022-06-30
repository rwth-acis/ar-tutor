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
    TextMeshProUGUI TMP_Hint;
    string initialMessage = "Scan your environment with the camera and place some objects from the menu on the right. Then, place a virtual agent on the floor and press \"Play\".";
    string hint;

    // Start is called before the first frame update
    void Start()
    {
        panel = this.gameObject;
        hint = "\n(Tap on this window to close it. Otherwise, it will disappear after " + seconds + " seconds.)";
        background = panel.transform.GetChild(0).gameObject;
        backgroundRT = background.GetComponent<RectTransform>();
        TMP_Text = panel.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TMP_Hint = panel.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        PrintInformation(initialMessage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintInformation(string information)
    {
        StartCoroutine(PrintInformationCoroutine(information, seconds));
    }

    IEnumerator PrintInformationCoroutine(string information, float seconds)
    {
        TMP_Text.text = information;
        TMP_Hint.text = hint;
        Debug.Log("TMP value: " + TMP_Text.GetPreferredValues().ToString());
        //backgroundRT.sizeDelta = new Vector2(TMP_Text.GetPreferredValues().x + 10, backgroundRT.sizeDelta.y);
        Debug.Log("Printed information: " + information);
        background.SetActive(true);

        yield return new WaitForSeconds(seconds);

        background.SetActive(false);
    }
}
