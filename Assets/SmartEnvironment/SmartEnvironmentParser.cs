using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnvironmentParser : MonoBehaviour
{
    [SerializeField] SmartObject[] smartObjects;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.SmartEnvironmentParsed(smartObjects);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
