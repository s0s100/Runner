using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class StoragePlayerManager
{
    private static string directoryPath = "SaveData";
    private static string fileName = "SaveData.txt";

    public static void Save(StoragePlayerData saveObj)
    {
        if (!DirectoryExists())
        {
            Directory.CreateDirectory(GetDirectoryPath());
        }

        try
        {
            string JSONdata = saveObj.ToJSON();
            File.WriteAllText(GetFullPath(), JSONdata);

            Debug.Log("Save path: " + GetFullPath());

        } catch (FileNotFoundException e)
        {
            Debug.LogError(e);
        }
    }

    public static void Load(StoragePlayerData saveObj)
    {
        if (DirectoryExists() && SaveFileExists())
        {
            try
            {
                string JSONdata = File.ReadAllText(GetFullPath());
                JsonUtility.FromJsonOverwrite(JSONdata, saveObj);

                Debug.Log("Load path: " + GetFullPath());

            } catch (SerializationException e)
            {
                Debug.LogError(e);
            }
        }
    }

    private static bool SaveFileExists()
    {
        return File.Exists(GetFullPath());
    }

    private static bool DirectoryExists()
    {
        return Directory.Exists(GetDirectoryPath());
    }

    private static string GetFullPath()
    {
        return Path.Combine(GetDirectoryPath(), fileName);
    }

    private static string GetDirectoryPath()
    {
        return Path.Combine(Application.persistentDataPath, directoryPath); 
    }
}
