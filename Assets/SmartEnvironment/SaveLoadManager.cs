using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveLoadManager
{
    public static void LoadOrInitializeSmartEnvironment()
    {
        // Saving and loading.
        if (File.Exists(Path.Combine(Application.persistentDataPath, "SmartEnvironment.json")))
        {
            Debug.Log("Found file SmartEnvironment.json, loading smart environment.");
            SmartEnvironment.LoadFromJSON(Path.Combine(
                Application.persistentDataPath, "SmartEnvironment.json"));
        }
        else
        {
            Debug.Log("Couldn't find SmartEnvironment.json, loading from template.");
            SmartEnvironment.InitializeFromDefault();
        }
    }

    public static void SaveSmartEnvironment()
    {
        SmartEnvironment.Instance.SaveToJSON(Path.Combine(
            Application.persistentDataPath, "SmartEnvironment.json"));

        Debug.Log("Saved:\n" + JsonUtility.ToJson(SmartEnvironment.Instance));
    }


    // Load from the default, for situations where we just want to reset.
    public static void LoadFromTemplate()
    {
        SmartEnvironment.InitializeFromDefault();
    }
}