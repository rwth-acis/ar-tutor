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
        smartEnvironmentAsJson = JsonUtility.ToJson(new JSONListWrapper<SmartObjectInstance>(smartEnvironment));
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
        SaveLoadManager.SaveSmartEnvironment();
    }

    // Load smart environment from the saved state or template.
    public void Load()
    {
        SaveLoadManager.LoadOrInitializeSmartEnvironment();
    }

    // Reset smart environment from the template.
    public void Reset()
    {
        SaveLoadManager.LoadFromTemplate();
    }

    public List<SmartObjectInstance> GetSmartObjectInstances()
    {
        return _instance.smartEnvironment;
    }
}


[System.Serializable]
public class JSONListWrapper<T>
{
    public List<T> list;
    public JSONListWrapper(List<T> list) => this.list = list;
}
