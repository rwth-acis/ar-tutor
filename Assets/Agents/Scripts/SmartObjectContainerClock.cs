using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObjectContainerClock : MonoBehaviour
{
    [SerializeField] SmartObject smartPointableObject;

    // Start is called before the first frame update
    void Start()
    {
        // Define the interactive area
        smartPointableObject.SetInteractiveArea(gameObject);
        // Signal to all components that a pointable object was instantiated
        //EventManager.WalkLabelInstantiated(smartDestinationObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPointableSOAffectedArea(GameObject affectedArea)
    {
        smartPointableObject.SetAffectedArea(affectedArea);
    }
}
