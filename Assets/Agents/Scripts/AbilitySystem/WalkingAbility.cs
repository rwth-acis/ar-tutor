using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Walking", menuName = "Abilities/Walking")]
public class WalkingAbility : Ability
{
    void OnEnable ()
    {
        base.flag = "Walking";
    }
}
