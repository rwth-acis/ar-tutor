using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Smart Object prototype.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "Generic", menuName = "Smart Objects/Generic")]
public class SmartObject : ScriptableObject
{
    public string nameSource;
    public string nameTarget;
    public string nameTarget_TowardsCase;
    public string nameTarget_AtCase;

    public GameObject physicalManifestation;
    public enum Placing { 
        Wall, Floor, Table
    }
    public Placing canBePlacedOn;
    public Sprite objectIconUI; 

    public GameObject interactiveArea = null;
    public GameObject affectedArea = null;

    private bool inUse = false;

    public List<Affordance> affordances;

    /// <summary>
    /// Check whether the affordances required for the interaction are provided.
    /// </summary>
	/// <param name="requiredAffordances">List of affordances required by the interaction.</param>
    /// <returns>True unless at least one of the required affordances is not provided.</returns>
    public bool CheckAffordances(List<Affordance> requiredAffordances)
    {
        // Return true...
        bool result = true;
        foreach (Affordance requiredAffordance in requiredAffordances)
        {
            // ...unless at least one of the required affordances is not provided
            if (affordances.Find(affordance => affordance.flag == requiredAffordance.flag) == null)
            {
                result = false;
            }
        }
        return result;
    }

    public void SetInteractiveArea(GameObject interactiveArea)
    {
        this.interactiveArea = interactiveArea;
    }

    public void SetAffectedArea(GameObject affectedArea)
    {
        this.affectedArea = affectedArea;
    }

    /// <summary>
    /// Check whether the Smart Object is virtual or physical. 
    /// </summary>
    /// <returns>True iff the Smart Object is virtual.</returns>
    public bool IsVirtual()
    {
        return this.physicalManifestation != null;
    }

    /// <summary>
    /// Check whether the virtual Smart Object contains the interactiveArea in its physical manifestation. 
    /// </summary>
    /// <returns>True iff the smart object contains the interactiveArea in its physical manifestation.</returns>
    public bool ContainsInteractiveArea()
    {
        return this.physicalManifestation == this.interactiveArea;
    }
}
