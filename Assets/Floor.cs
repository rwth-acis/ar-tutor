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
        StartCoroutine(SignalFloorInstantiation(1f));
    }

    // Coroutine test
    private IEnumerator SignalFloorInstantiation(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        EventManager.FloorInstantiated();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
