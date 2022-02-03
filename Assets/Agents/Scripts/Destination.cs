using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Signal to all components that a walk label was instantiated
        EventManager.WalkLabelInstantiated(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
