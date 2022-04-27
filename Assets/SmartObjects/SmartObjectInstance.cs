using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class that holds a real instance of a ScriptableObject smart object in the scene.
// Enables multiple smart object copies with mutable data.
[System.Serializable]
public class SmartObjectInstance
{
    // Reference to the scriptable object "template"
    public SmartObject smartObject;
    // Object-specific data
    public GameObject physicalManifestation;
    public GameObject interactiveArea;
    public GameObject affectedArea;
    // Object-ID?

    public SmartObjectInstance(SmartObject smartObject, GameObject physicalManifestation, GameObject interactiveArea, GameObject affectedArea)
    {
        this.smartObject = smartObject;
        this.physicalManifestation = physicalManifestation;
        this.interactiveArea = interactiveArea;
        this.affectedArea = affectedArea;
    }
}