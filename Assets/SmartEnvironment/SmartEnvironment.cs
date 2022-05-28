using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspired by http://toqoz.fyi/unity-painless-inventory.html
// Saving using unity dev example by Richard Fine
[CreateAssetMenu(menuName = "SmartEnvironment", fileName = "SmartEnvironment.asset")]
[System.Serializable]
public class SmartEnvironment : ScriptableObject
{
    private static SmartEnvironment _instance;
    public static SmartEnvironment Instance
    {
        get
        {
            if (!_instance)
            {
                SmartEnvironment[] tmp = Resources.FindObjectsOfTypeAll<SmartEnvironment>();
                if (tmp.Length > 0)
                {
                    _instance = tmp[0];
                    Debug.Log("Found smart environment as: " + _instance);
                }
                else
                {
                    Debug.Log("Did not find smart environment, loading from file or template.");
                    SaveLoadManager.LoadOrInitializeSmartEnvironment();
                }
            }
            return _instance;
        }
    }

    public static void InitializeFromDefault()
    {
        if (_instance) DestroyImmediate(_instance);
        _instance = Instantiate((SmartEnvironment)Resources.Load("SmartEnvironmentTemplate"));
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    public static void LoadFromJSON(string path)
    {
        if (_instance) DestroyImmediate(_instance);
        _instance = ScriptableObject.CreateInstance<SmartEnvironment>();
        JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), _instance);
        _instance.smartEnvironment = JsonUtility.FromJson<JSONListWrapper<SmartObjectInstance>>(_instance.smartEnvironmentAsJson).list;
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    public void SaveToJSON(string path)
    {
        _instance.smartEnvironmentAsJson = JsonUtility.ToJson(new JSONListWrapper<SmartObjectInstance>(_instance.smartEnvironment));
        Debug.LogFormat("Saving smart environment to {0}", path);
        System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
    }

    /* Inventory START */
    public List<SmartObjectInstance> smartEnvironment;
    public string smartEnvironmentAsJson;

    // Remove an item at an index
    public void RemoveSmartObject(int index)
    {
        _instance.smartEnvironment.RemoveAt(index);
    }

    // Insert a smart object, return the index where it was inserted
    public int InsertSmartObject(SmartObjectInstance smartObjectInstance)
    {
        _instance.smartEnvironment.Add(smartObjectInstance);
        return _instance.smartEnvironment.IndexOf(smartObjectInstance);
    }

    // Get a smart object instance if it exists.
    public SmartObjectInstance GetSmartObjectInstance(int index)
    {
        return _instance.smartEnvironment[index];
    }

    // Save the state of smart environment.
    public void Save()
    {
        //StressTest();
        SaveLoadManager.SaveSmartEnvironment();
        Debug.Log("There are currently " + _instance.smartEnvironment.Count + " Smart Objects in the Smart Environment.");
    }

    void StressTest()
    {
        List<SmartObjectInstance> tempSmartEnvironment = new List<SmartObjectInstance>();
        foreach (SmartObjectInstance smartObjectInstance in _instance.smartEnvironment)
        {
            for (int i = 0; i < 100; i++)
                tempSmartEnvironment.Add(smartObjectInstance);
        }
        _instance.smartEnvironment = tempSmartEnvironment;
    }

    // Load smart environment from the saved state or template.
    public void Load()
    {
        SaveLoadManager.LoadOrInitializeSmartEnvironment();
        Debug.Log("There are currently " + _instance.smartEnvironment.Count + " Smart Objects in the Smart Environment.");
    }

    // Reset smart environment from the template.
    public void Reset()
    {
        Debug.Log("Pre-reset: There are currently " + _instance.smartEnvironment.Count + " Smart Objects in the Smart Environment.");
        foreach (SmartObjectInstance smartObjectInstance in _instance.smartEnvironment)
        {
            Debug.Log(smartObjectInstance.smartObject.nameSource);
        }
        SaveLoadManager.LoadFromTemplate();
        //TODO soft empty, where just the instantiated objects' properties get cleared
        _instance.Empty();
        Debug.Log("Post-reset: There are currently " + _instance.smartEnvironment.Count + " Smart Objects in the Smart Environment.");
    }

    public List<SmartObjectInstance> GetSmartObjectInstances()
    {
        return _instance.smartEnvironment;
    }

    public int GetSmartObjectInstanceIndex(SmartObjectInstance smartObjectInstance)
    {
        return _instance.smartEnvironment.IndexOf(smartObjectInstance);
    }

    public void Empty()
    {
        _instance.smartEnvironment.Clear();
        _instance.smartEnvironmentAsJson = "";
    }
}


[System.Serializable]
public class JSONListWrapper<T>
{
    public List<T> list;
    public JSONListWrapper(List<T> list) => this.list = list;
}
