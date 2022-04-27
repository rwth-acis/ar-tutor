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
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    public void SaveToJSON(string path)
    {
        Debug.LogFormat("Saving smart environment to {0}", path);
        System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
    }

    /* Inventory START */
    public List<SmartObjectInstance> smartEnvironment;

    /*public bool SlotEmpty(int index)
    {
        if (smartEnvironment[index] == null || smartEnvironment[index].smartObject == null)
            return true;

        return false;
    }

    // Get a smart object instance if it exists.
    public bool GetSmartObjectInstance(int index, out SmartObjectInstance smartObjectInstance)
    {
        // smartEnvironment[index] doesn't return null, so check item instead.
        if (SlotEmpty(index))
        {
            item = null;
            return false;
        }

        item = inventory[index];
        return true;
    }

    // Remove an item at an index if one exists at that index.
    public bool RemoveItem(int index)
    {
        if (SlotEmpty(index))
        {
            // Nothing existed at the specified slot.
            return false;
        }

        inventory[index] = null;

        return true;
    }

    // Insert an item, return the index where it was inserted.  -1 if error.
    public int InsertItem(ItemInstance item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (SlotEmpty(i))
            {
                inventory[i] = item;
                return i;
            }
        }

        // Couldn't find a free slot.
        return -1;
    }*/

    // Remove an item at an index
    public void RemoveSmartObject(int index)
    {
        smartEnvironment.RemoveAt(index);
    }

    // Insert a smart object, return the index where it was inserted
    public int InsertSmartObject(SmartObjectInstance smartObjectInstance)
    {
        smartEnvironment.Add(smartObjectInstance);
        return smartEnvironment.IndexOf(smartObjectInstance);
    }

    // Get a smart object instance if it exists.
    public SmartObjectInstance GetSmartObjectInstance(int index)
    {
        return smartEnvironment[index];
    }

    // Simply save.
    public void Save()
    {
        SaveLoadManager.SaveSmartEnvironment();
    }

    // Simply load.
    public void Load()
    {
        SaveLoadManager.LoadOrInitializeSmartEnvironment();
    }
}