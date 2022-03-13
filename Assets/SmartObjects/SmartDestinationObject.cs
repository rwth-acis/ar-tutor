using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAgentsFramework;

[CreateAssetMenu(fileName = "Destination", menuName = "Smart Objects/Destination")]
public class SmartDestinationObject : SmartObject
{
    void OnEnable ()
    {
        base.affordance = "Walking";
    }
}
