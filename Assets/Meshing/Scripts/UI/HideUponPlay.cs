using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUponPlay : MonoBehaviour
{
    [SerializeField]
    GameObject[] UI_elements;

    private void OnEnable()
    {
        // Register to event manager events
        EventManager.OnSXStatusChanged += ChangeUIVisibility;
    }

    private void OnDisable()
    {
        // Unregister from event manager events
        EventManager.OnSXStatusChanged -= ChangeUIVisibility;
    }

    private void ChangeUIVisibility(bool play)
    {
        foreach (GameObject UI_element in UI_elements)
        {
            UI_element.SetActive(play);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
