using UnityEngine;
using System.IO;
using System.Runtime.Serialization;

public static class StoragePlayerManager
{
    private static string directoryPath = "SaveData";
    private static string fileName = "SaveData.txt";
    
    public static void Save(StoragePlayerData saveObj)
    {
        if (!DirectoryExists())
        {
            Debug.Log("Created directory at the path:" + GetDirectoryPath());
            Directory.CreateDirectory(GetDirectoryPath());
        }
        try
        {
            Debug.Log("Saving data file");
            string JSONdata = saveObj.ToJSON();
            File.WriteAllText(GetFullPath(), JSONdata);

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
                Debug.Log("Loading data file");
                string JSONdata = File.ReadAllText(GetFullPath());
                JsonUtility.FromJsonOverwrite(JSONdata, saveObj);

            } catch (SerializationException e)
            {
                Debug.LogError(e);
            }
        } else
        {
            Debug.Log("Storage player data does not exist");
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
