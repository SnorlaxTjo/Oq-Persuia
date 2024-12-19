using System.Diagnostics.Contracts;
using UnityEngine;

public class ProgressKeeper : MonoBehaviour
{
    public static ProgressKeeper instance;

    [SerializeField] GameObject[] dialogueObjects;
    [SerializeField] Collider[] teleporters;
    [SerializeField] PlayerData playerData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Save();
        }
    }

    public void Save()
    {
        bool[] dialogueObjectsActive = new bool[dialogueObjects.Length];
        bool[] teleportersActive = new bool[teleporters.Length];

        for (int i = 0; i < dialogueObjects.Length; i++)
        {
            dialogueObjectsActive[i] = dialogueObjects[i].activeSelf;
        }

        for (int i = 0; i < teleporters.Length; i++)
        {
            teleportersActive[i] = teleporters[i].isTrigger;
        }

        string dialogueHexa = BinaryHexaConverter.ConvertBinaryToHexa(dialogueObjectsActive);
        string teleporterHexa = BinaryHexaConverter.ConvertBinaryToHexa(teleportersActive);

        string completeData = dialogueHexa + "ÿ" + teleporterHexa;
        SaveSystem.SaveData(completeData);
    }

    public void Load()
    {
        string completeData = SaveSystem.LoadData();

        if (completeData != null)
        {
            string[] hexaStrings = completeData.Split('ÿ');

            bool[] dialogueObjectsActive = new bool[dialogueObjects.Length];
            bool[] teleportersActive = new bool[teleporters.Length];

            dialogueObjectsActive = BinaryHexaConverter.ConvertHexaToBinary(hexaStrings[0], dialogueObjectsActive.Length);
            teleportersActive = BinaryHexaConverter.ConvertHexaToBinary(hexaStrings[1], teleportersActive.Length);

            for (int i = 0; i < dialogueObjectsActive.Length; i++)
            {
                dialogueObjects[i].SetActive(dialogueObjectsActive[i]);
            }

            for (int i = 0; i < teleportersActive.Length; i++)
            {
                teleporters[i].isTrigger = teleportersActive[i];
            }

            Debug.Log("a");
        }
    }
}

[System.Serializable]
public struct PlayerData
{
    public int worldId;
    public Vector3 playerPosition;
    public PlayerBool[] playerBool;

    public PlayerData(int worldId, Vector3 playerPosition, PlayerBool[] playerBool)
    {
        this.worldId = worldId;
        this.playerPosition = playerPosition;
        this.playerBool = playerBool;
    }
}

[System.Serializable]
public struct PlayerBool
{
    public string name;
    public bool isActive;

    public PlayerBool(string name, bool isActive)
    {
        this.name = name;
        this.isActive = isActive;
    }
}