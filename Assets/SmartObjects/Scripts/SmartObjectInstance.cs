using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class that holds a real instance of a ScriptableObject smart object in the scene.
// Enables multiple smart object copies with mutable data.
[Serializable]
public class SmartObjectInstance
{
    // Reference to the scriptable object "template"
    public SmartObject smartObject = null;
    // Object-specific data
    public InstanceTransform physicalManifestation = null;
    public InstanceTransform interactiveArea = null;
    public InstanceTransform affectedArea = null;
    public bool busy = false;
    public GameObject button = null;
    public bool instantiated = false;
    // Objects for referencing during the same app run
    public GameObject physicalManifestationGameObject { get; set; }
    public GameObject interactiveAreaGameObject { get; set; }
    public GameObject affectedAreaGameObject { get; set; }
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

    // Checks whether the smart object affords what is required by the interaction
    // Considers provided interfaces of the smart object as well as the state its instance is in
    public bool CheckAffordances(List<Affordance> requiredAffordances)
    {
        if (busy == false && smartObject.CheckAffordances(requiredAffordances))
            return true;
        else
            return false;

        /*if (busy == true) 
            return false; //TODO throw exception
        else if (smartObject.CheckAffordances(interaction.requiredAffordances))
            return true;
        else
            return false;*/
    }


}

[Serializable]
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