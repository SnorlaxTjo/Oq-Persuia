using UnityEngine;
using System.IO;

public static class SaveSystem
{
    static readonly string fileName = "/bd2jzq6.oq";

    public static string FileName { get { return fileName; } }

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
            return null;
        }
        
    }
}
