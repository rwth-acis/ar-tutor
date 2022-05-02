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
            Destroy(eachObject.gameObject);
            //eachObject.gameObject.SetActive(false);
        }
        //TODO rename "smartEnvironment" to "objectCollection"
        //TODO move the activation method to a SmartObjectInstanceTools class
        //foreach (SmartObjectInstance smartObjectInstance in SmartEnvironment.Instance.smartEnvironment)
        foreach (SmartObjectInstance smartObjectInstance in SmartEnvironment.Instance.GetSmartObjectInstances())
        {
            GameObject restoredPhysicalManifestation = Instantiate(smartObjectInstance.smartObject.physicalManifestation, smartEnvironmentTransform);
            smartObjectInstance.physicalManifestation.ApplyTransformTo(restoredPhysicalManifestation.transform);

            GameObject restoredInteractiveArea = Instantiate(smartObjectInstance.smartObject.interactiveArea, smartEnvironmentTransform);
            smartObjectInstance.interactiveArea.ApplyTransformTo(restoredInteractiveArea.transform);

            GameObject restoredAffectedArea = Instantiate(smartObjectInstance.smartObject.affectedArea, smartEnvironmentTransform);
            smartObjectInstance.affectedArea.ApplyTransformTo(restoredAffectedArea.transform);
            /*smartObjectInstance.physicalManifestation.transform.parent = smartEnvironmentTransform;
            smartObjectInstance.physicalManifestation.SetActive(true);
            //Instantiate(smartObjectInstance.interactiveArea, smartEnvironmentTransform);
            smartObjectInstance.interactiveArea.transform.parent = smartEnvironmentTransform;
            smartObjectInstance.interactiveArea.SetActive(true);
            //Instantiate(smartObjectInstance.affectedArea, smartEnvironmentTransform);
            smartObjectInstance.affectedArea.transform.parent = smartEnvironmentTransform;
            smartObjectInstance.affectedArea.SetActive(true);*/
            Debug.Log("Restored a smart object instance");
        }
    }
}
