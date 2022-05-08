using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PointingAffordance", menuName = "Affordances/Pointing Affordance")]
public class PointingAffordance : Affordance
{
    void OnEnable()
    {
        base.flag = "Pointing";
    }
}
