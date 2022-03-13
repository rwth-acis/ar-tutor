using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SmartObject : ScriptableObject
{

    public GameObject interactiveArea = null;

    public GameObject affectedArea = null;

    private bool inUse = false;

    protected string affordance;

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
}
