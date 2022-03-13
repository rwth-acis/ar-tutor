using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAbilities : MonoBehaviour
{
    public Ability[] abilities;

    public bool CheckAbility(string flag)
    {
        bool result = false;
        foreach (Ability ability in abilities)
        {
            if (ability.flag == flag)
                result = true;
        }
        return result;
    }
}
