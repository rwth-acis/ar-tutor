using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Floor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Signal to all components that a walk label was instantiated
        EventManager.FloorInstantiated(GetComponent<NavMeshSurface>());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
