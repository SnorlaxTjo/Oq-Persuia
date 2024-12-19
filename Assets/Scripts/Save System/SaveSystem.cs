using UnityEngine;
using System.IO;
using JetBrains.Annotations;

public static class SaveSystem
{
    static string fileName = "/01.oq";

    public static void SaveData(string completeData)
    {
        string path = Application.persistentDataPath + fileName;
        File.WriteAllText(path, completeData);
    }

    public static string LoadData()
    {
        string path = Application.persistentDataPath + fileName;
        if (File.Exists(path))
        {
            string completeData = File.ReadAllText(path);
            return completeData;
        }
        else
        {
            Debug.LogWarning("Save File not found in " + path);
            return null;
        }
        
    }
}
