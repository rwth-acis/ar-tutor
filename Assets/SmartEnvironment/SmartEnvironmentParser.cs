using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnvironmentParser : MonoBehaviour
{
    [TextArea] [SerializeField] string despription;

    [SerializeField] SmartObject[] smartObjects;

    // Start is called before the first frame update
    void Start()
    {
        ParseSmartEnvironment();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParseSmartEnvironment()
    {
        EventManager.SmartEnvironmentParsed(smartObjects);
    }
}
