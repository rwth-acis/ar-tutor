using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAbilities : MonoBehaviour
{
    public List<Ability> abilities;

    public bool CheckAbilities(List<Ability> requiredAbilities)
    {
        // Return true...
        bool result = true;
        foreach (Ability requiredAbility in requiredAbilities)
        {
            // ...unless at least one of the required abilities is not provided
            if (abilities.Find(ability => ability.flag == requiredAbility.flag) == null)
            {
                result = false;
                //TODO throw exception
            }
        }
        return result;
    }
}
