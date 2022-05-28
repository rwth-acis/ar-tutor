using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Generic", menuName = "Smart Objects/Generic")]
public class SmartObject : ScriptableObject
{
    public string nameSource;
    public string nameTarget;

    public GameObject physicalManifestation;
    public enum Placing { //TODO What is with vitual objects, etc? Maybe have one menu and just grey out objects that can not be placed upon certain classification at all.
        Wall, Floor, Table
    }
    public Placing canBePlacedOn;
    public Sprite objectIconUI; //TODO Set standard as a placeholder image

    // public ISmartObjectInstantiatable instantiatorScript; // Script that gets appended to the smart object's physical
                                                             // manifestation in the scene and sets up the affected and interactive areas at according positions.
                                                             // It would be easiest though to be able to assign everything in the inspector... But do I provide
                                                             // the positions there then or just parts of the prefab or what?

    public GameObject interactiveArea = null;
    public GameObject affectedArea = null;

    /*
    public float interactiveAreaWidth = -1f;
    public float interactiveAreaLength = -1f;
    public float affectedAreaWidth = -1f;
    public float affectedAreaLength = -1f;
    */

    private bool inUse = false;

    public string affordance;

    // False means the object is NOT in use
    public bool CheckAffordance(string flag)
    {
        return affordance == flag;
    }

    // False means the object is NOT in use
    public bool CheckState()
    {
        return inUse;
    }

    public void SetInteractiveArea(GameObject interactiveArea)
    {
        this.interactiveArea = interactiveArea;
    }

    public void SetAffectedArea(GameObject affectedArea)
    {
        this.affectedArea = affectedArea;
    }

    // Returns true iff the smart object is virtual
    public bool IsVirtual()
    {
        return this.physicalManifestation != null;
    }

    // Returns true iff the smart object is virtual
    public bool ContainsInteractiveArea()
    {
        return this.physicalManifestation == this.interactiveArea;
    }
}
