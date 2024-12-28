using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CheckForSaveFile : MonoBehaviour
{
    [SerializeField] Button continueGameButton;

    private void Start()
    {
        string fileName = SaveSystem.FileName;

        bool fileExists = File.Exists(Application.persistentDataPath + fileName);
        continueGameButton.interactable = fileExists;
    }

    public void RemoveSaveFile()
    {
        string path = Application.persistentDataPath + SaveSystem.FileName;

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
