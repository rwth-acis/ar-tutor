using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class that holds a real instance of a ScriptableObject smart object in the scene.
// Enables multiple smart object copies with mutable data.
[System.Serializable]
public class SmartObjectInstance
{
    // Reference to the scriptable object "template"
    public SmartObject smartObject = null;
    // Object-specific data
    public InstanceTransform physicalManifestation = null;
    public InstanceTransform interactiveArea = null;
    public InstanceTransform affectedArea = null;
    public GameObject button = null;
    public bool instantiated = false;
    // Object-ID?

    public SmartObjectInstance(SmartObject smartObject, Transform physicalManifestation, Transform interactiveArea, Transform affectedArea)
    {
        this.smartObject = smartObject;
        //TODO Add actual runtime game objects for planning the behavioral sequences and stuff
        this.physicalManifestation = new InstanceTransform(physicalManifestation);
        this.interactiveArea = new InstanceTransform(interactiveArea);
        this.affectedArea = new InstanceTransform(affectedArea);
    }

    public SmartObjectInstance(SmartObject smartObject)
    {
        this.smartObject = smartObject;
    }
}

[System.Serializable]
public class InstanceTransform
{
    public Vector3 localPosition = Vector3.zero;
    public Quaternion localRotation = Quaternion.identity;
    public Vector3 localEulerAngles = Vector3.zero;
    public Vector3 localScale = Vector3.one;

    public InstanceTransform(Transform transform)
    {
        this.localPosition = transform.localPosition;
        this.localRotation = transform.localRotation;
        this.localEulerAngles = transform.localEulerAngles;
        if(transform.localScale != Vector3.zero)
            this.localScale = transform.localScale;
    }

    // For the case when DOScale is being used on the prefab
    public InstanceTransform(Transform transform, Vector3 finalScale)
    {
        this.localPosition = transform.localPosition;
        this.localRotation = transform.localRotation;
        this.localEulerAngles = transform.localEulerAngles;
        this.localScale = finalScale;
    }

    public void ApplyTransformTo(Transform transform)
    {
        transform.localPosition = this.localPosition;
        transform.localRotation = this.localRotation;
        transform.localEulerAngles = this.localEulerAngles;
        transform.localScale = this.localScale;
    }
}