using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObjectContainer : MonoBehaviour
{
    [SerializeField] SmartObject smartDestinationObject;

    // Start is called before the first frame update
    void Start()
    {
        // Define the interactive area
        smartDestinationObject.SetInteractiveArea(gameObject);
        // Signal to all components that a walk label was instantiated
        EventManager.WalkLabelInstantiated(smartDestinationObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
