using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that represents a Smart Environment that serves as a singleton inventory for Smart Objects.
/// </summary>
// Inspired by http://toqoz.fyi/unity-painless-inventory.html
// Saving using a Unity dev example by Richard Fine
[CreateAssetMenu(menuName = "SmartEnvironment", fileName = "SmartEnvironment.asset")]
[System.Serializable]
public class SmartEnvironment : ScriptableObject
{
    private static SmartEnvironment _instance;
    /// <summary>
    /// Smart Environment singleton.
    /// </summary>
    public static SmartEnvironment Instance
    {
        get
        {
            if (!_instance)
            {
                SmartEnvironment[] tmp = Resources.FindObjectsOfTypeAll<SmartEnvironment>();
                Debug.Log("tmp.Length (number of smart environments): " + tmp.Length.ToString());
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

    /// <summary>
    /// Initialize the Smart Environment from the template.
    /// </summary>
    public static void InitializeFromDefault()
    {
        if (_instance) DestroyImmediate(_instance);
        _instance = Instantiate((SmartEnvironment)Resources.Load("SmartEnvironmentTemplate"));
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    /// <summary>
    /// Load the Smart Environment from the JSON file.
    /// </summary>
	/// <param name="path">Path of the JSON file.</param>
    public static void LoadFromJSON(string path)
    {
        if (_instance) DestroyImmediate(_instance);
        _instance = ScriptableObject.CreateInstance<SmartEnvironment>();
        JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), _instance);
        //_instance.smartEnvironment = JsonUtility.FromJson<JSONListWrapper<SmartObjectInstance>>(_instance.smartEnvironmentAsJson).list;
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    /// <summary>
    /// Save the Smart Environment to the JSON file.
    /// </summary>
	/// <param name="path">Path of the JSON file.</param>
    public void SaveToJSON(string path)
    {
        Debug.LogFormat("Saving smart environment to {0}", path);
        System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
    }

    /// <summary>
    /// Save an empty Smart Environment to the JSON file.
    /// </summary>
	/// <param name="path">Path of the JSON file.</param>
    public void SaveEmptyToJSON(string path)
    {
        Debug.LogFormat("Saving smart environment to {0}", path);
        System.IO.File.WriteAllText(path, "{\"smartEnvironment\":[]}");
    }

    /* Inventory START */
    public List<SmartObjectInstance> smartEnvironment;

    /// <summary>
    /// Remove an item at an index
    /// </summary>
    /// <param name="index">Index of the item to be removed.</param>
    public void RemoveSmartObject(int index)
    {
        smartEnvironment.RemoveAt(index);
    }

    /// <summary>
    /// Insert a Smart Object instance, return the index where it was inserted.
    /// </summary>
    /// <returns>Index in the Smart Environment where the Smart Object instance was inserted.</returns>
    public int InsertSmartObject(SmartObjectInstance smartObjectInstance)
    {
        smartEnvironment.Add(smartObjectInstance);
        return smartEnvironment.IndexOf(smartObjectInstance);
    }

    /// <summary>
    /// Get a Smart Object instance if it exists.
    /// </summary>
	/// <param name="index">Index of the requested Smart Object instance.</param>
    /// <returns>Smart Object instance or null.</returns>
    public SmartObjectInstance GetSmartObjectInstance(int index)
    {
        return smartEnvironment[index];
    }

    /// <summary>
    /// Save the state of the Smart Environment.
    /// </summary>
    public void Save()
    {
        //StressTest();
        SaveLoadManager.SaveSmartEnvironment();
        Debug.Log("There are currently " + smartEnvironment.Count + " Smart Objects in the Smart Environment.");
        EventManager.PostStatement("user", "saved", "SE");
    }

    /// <summary>
    /// Stress test from the thesis evaluation.
    /// </summary>
    void StressTest()
    {
        List<SmartObjectInstance> tempSmartEnvironment = new List<SmartObjectInstance>();
        foreach (SmartObjectInstance smartObjectInstance in smartEnvironment)
        {
            for (int i = 0; i < 100; i++)
                tempSmartEnvironment.Add(smartObjectInstance);
        }
        smartEnvironment = tempSmartEnvironment;
    }

    /// <summary>
    /// Load the Smart Environment from the saved state or a template.
    /// </summary>
    public void Load()
    {
        SaveLoadManager.LoadOrInitializeSmartEnvironment();
        Debug.Log("There are currently " + smartEnvironment.Count + " Smart Objects in the Smart Environment.");
        EventManager.PostStatement("user", "loaded", "SE");
    }

    /// <summary>
    /// Reset the Smart Environment from the template.
    /// </summary>
    public void Reset()
    {
        Debug.Log("Pre-reset: There are currently " + smartEnvironment.Count + " Smart Objects in the Smart Environment.");
        SaveLoadManager.LoadFromTemplate();
        Debug.Log("Post-reset: There are currently " + smartEnvironment.Count + " Smart Objects in the Smart Environment.");
        EventManager.PostStatement("user", "reset", "SE");
    }

    /// <summary>
    /// Get the list of the Smart Object instances that are currentlu in the Smart Environment.
    /// </summary>
    public List<SmartObjectInstance> GetSmartObjectInstances()
    {
        return smartEnvironment;
    }

    /// <summary>
    /// Get the index of a Smart Object instance if it exists.
    /// </summary>
	/// <param name="smartObjectInstance">Requested Smart Object instance.</param>
    /// <returns>Index of the Smart Object instance or -1.</returns>
    public int GetSmartObjectInstanceIndex(SmartObjectInstance smartObjectInstance)
    {
        return smartEnvironment.IndexOf(smartObjectInstance);
    }

    /// <summary>
    /// Clear the Smart Environment.
    /// </summary>
    public void EmptySE()
    {
        smartEnvironment.Clear();
        // Save the Smart Environment to JSON
        SaveLoadManager.SaveSmartEnvironment();
    }
}
