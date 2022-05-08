using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrabbingAffordance", menuName = "Affordances/Grabbing Affordance")]
public class GrabbingAffordance : Affordance
{
    void OnEnable()
    {
        base.flag = "Grabbing";
    }
}
