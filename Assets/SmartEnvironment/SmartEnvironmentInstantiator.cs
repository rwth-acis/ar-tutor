using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnvironmentInstantiator : MonoBehaviour
{
    [SerializeField]
    private Transform smartEnvironmentTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Instantiate()
    {
        // First, remove all the existing GameObjects
        foreach (Transform eachObject in smartEnvironmentTransform)
        {
            //Destroy(eachObject.gameObject);
            eachObject.gameObject.SetActive(false);
        }
        //TODO rename "smartEnvironment" to "objectCollection"
        //TODO move the activation method to a SmartObjectInstanceTools class
        foreach (SmartObjectInstance smartObjectInstance in SmartEnvironment.Instance.smartEnvironment)
        {
            //Instantiate(smartObjectInstance.physicalManifestation, smartEnvironmentTransform);
            smartObjectInstance.physicalManifestation.transform.parent = smartEnvironmentTransform;
            smartObjectInstance.physicalManifestation.SetActive(true);
            //Instantiate(smartObjectInstance.interactiveArea, smartEnvironmentTransform);
            smartObjectInstance.interactiveArea.transform.parent = smartEnvironmentTransform;
            smartObjectInstance.interactiveArea.SetActive(true);
            //Instantiate(smartObjectInstance.affectedArea, smartEnvironmentTransform);
            smartObjectInstance.affectedArea.transform.parent = smartEnvironmentTransform;
            smartObjectInstance.affectedArea.SetActive(true);
            Debug.Log("Instantiated a smart object component");
        }
    }
}
