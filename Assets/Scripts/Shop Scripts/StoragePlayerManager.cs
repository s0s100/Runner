using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class StoragePlayerManager
{
    private static string directoryPath = "SaveData";
    private static string fileName = "SaveData.comp3";

    public static void Save(StoragePlayerData saveObj)
    {
        if (!DirectoryExists())
        {
            Directory.CreateDirectory(GetDirectoryPath());
        }

        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Create(GetFullPath());
            binaryFormatter.Serialize(fileStream, saveObj);
            fileStream.Close();
        } catch (FileNotFoundException e)
        {
            Debug.LogError(e);
        }
    }

    public static StoragePlayerData Load()
    {
        if (DirectoryExists() && SaveFileExists())
        {
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = File.Open(GetFullPath(), FileMode.Open);
                StoragePlayerData playerData = (StoragePlayerData)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();

                return playerData;
            } catch (SerializationException e)
            {
                Debug.LogError(e);
            }
        }

        return null;
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
