using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressKeeper : MonoBehaviour
{
    public static ProgressKeeper instance;

    [SerializeField] Teleporter spawnTeleporter;
    [SerializeField] GameObject spawnTeleporterPlace;
    [SerializeField] float playerYTeleportOffset;

    [Space]

    [SerializeField] GameObject[] dialogueObjects;
    [SerializeField] Collider[] teleporters;
    [SerializeField] PlayerData playerData;
    [SerializeField] RectTransform mapGoalMarker;
    [SerializeField] Collider[] triggerOnTouch;

    [Space]

    [SerializeField] float timeBetweenAutoSave;
    [SerializeField] GameObject saveText;
    [SerializeField] Button[] saveButtonsInPauseMenu;

    float timeSinceAutoSave;
    bool canSave;
    bool enableAutoSave = true;

    public GameObject SpawnTeleporterPlace { get { return spawnTeleporterPlace; } }
    public float TimeBetweenAutoSave { get { return timeBetweenAutoSave; } set { timeBetweenAutoSave = value; } }
    public bool CanSave { get { return canSave; } }
    public bool EnableAutoSave { get { return enableAutoSave; } set {  enableAutoSave = value; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        canSave = true;
    }

    private void Start()
    {
        Load();
        enableAutoSave = Options.instance.Autosave;
        timeBetweenAutoSave = Options.instance.CurrentAutosaveTime * 60;
    }

    public void EnableSave(bool enable)
    {
        canSave = enable;
        foreach(Button button in saveButtonsInPauseMenu)
        {
            button.interactable = enable;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Save();
        }

        if (!enableAutoSave) { return; }

        timeSinceAutoSave += Time.deltaTime;
        if (timeSinceAutoSave > timeBetweenAutoSave - 2 && timeSinceAutoSave < timeBetweenAutoSave)
        {
            saveText.SetActive(true);
        }

        if (timeSinceAutoSave >= timeBetweenAutoSave)
        {
            Save();
            timeSinceAutoSave -= timeBetweenAutoSave;
            saveText.SetActive(false);
        }
    }

    public void Save()
    {
        if (!canSave) { return; }

        #region Change In GameObject Bools
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
        #endregion

        #region Player Bools
        for (int i = 0; i < playerData.playerBool.Length; i++)
        {
            switch (playerData.playerBool[i].name)
            {
                case "HasObtainedMap":
                    playerData.playerBool[i].value = FindObjectOfType<UIManager>().HasGottenMap;
                    break;

                case "HasObtainedPhone":
                    playerData.playerBool[i].value = FindObjectOfType<UIManager>().HasObtainedPhone;
                    break;

                case "HasObtainedBow":
                    playerData.playerBool[i].value = FindObjectOfType<Bow>().HasObtainedBow;
                    break;
            }
        }

        bool[] playerBools = new bool[playerData.playerBool.Length];
        for (int i = 0; i < playerBools.Length; i++)
        {
            playerBools[i] = playerData.playerBool[i].value;
        }
        #endregion

        #region Player Position
        playerData.worldId = FindObjectOfType<WorldManager>().CurrentWorld;
        playerData.playerPosition = FindObjectOfType<PlayerController>().transform.position;
        playerData.playerPosition += new Vector3(0, playerYTeleportOffset, 0);

        string playerVectorString = string.Empty;

        Vector3 playerPosition = playerData.playerPosition;
        if (playerPosition.x >= 0)
        {
            playerVectorString += "A" + Mathf.Abs((int)playerPosition.x);
        }
        else
        {
            playerVectorString += "B" + Mathf.Abs((int)playerPosition.x);
        }

        if (playerPosition.y >= 0)
        {
            playerVectorString += "A" + Mathf.Abs((int)playerPosition.y + 1);
        }
        else
        {
            playerVectorString += "B" + Mathf.Abs((int)playerPosition.y + 1);
        }

        if (playerPosition.z >= 0)
        {
            playerVectorString += "A" + Mathf.Abs((int)playerPosition.z);
        }
        else
        {
            playerVectorString += "B" + Mathf.Abs((int)playerPosition.z);
        }

        string playerPositionString = playerData.worldId.ToString() + "E" + playerVectorString;
        #endregion

        #region Map Goal Marker
        string markerPositionString = string.Empty;
        Vector2 markerPosition = mapGoalMarker.position;

        if (markerPosition.x >= 0)
        {
            markerPositionString += "A" + Mathf.Abs((int)markerPosition.x);
        }
        else
        {
            markerPositionString += "B" + Mathf.Abs((int)markerPosition.x);
        }

        if (markerPosition.y >= 0)
        {
            markerPositionString += "A" + Mathf.Abs((int)markerPosition.y);
        }
        else
        {
            markerPositionString += "B" + Mathf.Abs((int)markerPosition.y);
        }
        #endregion

        #region Locations Been To
        WorldManager worldManager = FindObjectOfType<WorldManager>();
        List<bool> hasBeenToPlace = new List<bool>();

        foreach (World world in worldManager.Worlds)
        {
            if (world.markerObject != null)
            {
                hasBeenToPlace.Add(world.markerObject.GetComponent<MapMarker>().HasVisited);
            }
        }

        bool[] hasVisitedPlaces = hasBeenToPlace.ToArray();
        #endregion

        #region Trigger On Touch Used
        bool[] touchTriggersActive = new bool[triggerOnTouch.Length];

        for (int i = 0;  i < touchTriggersActive.Length; i++)
        {
            touchTriggersActive[i] = triggerOnTouch[i].enabled;
        }
        #endregion

        string dialogueHexa = BinaryHexaConverter.ConvertBinaryToHexa(dialogueObjectsActive);
        string teleporterHexa = BinaryHexaConverter.ConvertBinaryToHexa(teleportersActive);

        string playerBoolHexa = BinaryHexaConverter.ConvertBinaryToHexa(playerBools);

        string hasVisitedHexa = BinaryHexaConverter.ConvertBinaryToHexa(hasVisitedPlaces);

        string hasTriggeredTouchHexa = BinaryHexaConverter.ConvertBinaryToHexa(touchTriggersActive);

        string completeData = dialogueHexa + "ÿ" + teleporterHexa + "ÿ" + playerBoolHexa + "ÿ" + playerPositionString + "ÿ" + markerPositionString + "ÿ" + hasVisitedHexa
            + "ÿ" + hasTriggeredTouchHexa;
        SaveSystem.SaveData(completeData);

        
    }

    public void Load()
    {
        string completeData = SaveSystem.LoadData();

        if (completeData != null)
        {
            string[] hexaStrings = completeData.Split('ÿ');

            #region Change In GameObject Bools
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
            #endregion

            #region Player Bools
            bool[] playerBools = new bool[playerData.playerBool.Length];

            playerBools = BinaryHexaConverter.ConvertHexaToBinary(hexaStrings[2], playerBools.Length);

            for (int i = 0; i < playerBools.Length; i++)
            {
                playerData.playerBool[i].value = playerBools[i];
            }

            for (int i = 0; i < playerData.playerBool.Length; i++)
            {
                switch (playerData.playerBool[i].name)
                {
                    case "HasObtainedMap":
                         FindObjectOfType<UIManager>().HasGottenMap = playerData.playerBool[i].value;
                        break;

                    case "HasObtainedPhone":
                        FindObjectOfType<UIManager>().HasObtainedPhone = playerData.playerBool[i].value;
                        FindObjectOfType<FastTravel>().HasGottenFastTravel = playerData.playerBool[i].value;
                        break;

                    case "HasObtainedBow":
                        FindObjectOfType<Bow>().HasObtainedBow = playerData.playerBool[i].value;
                        break;
                }
            }
            #endregion

            #region Player Position
            string[] splitNumbers = hexaStrings[3].Split('E');

            playerData.worldId = int.Parse(splitNumbers[0]);

            char[] playerChars = splitNumbers[1].ToCharArray();
            string[] playerVectorValues = new string[3];
            int[] playerVectorValuesInt = new int[3];
            int currentPosition = -1;

            foreach (char c in playerChars)
            {
                if (int.TryParse(c.ToString(), out int number))
                {
                    playerVectorValues[currentPosition] += number.ToString();
                }
                else
                {
                    currentPosition++;
                    if (c == 'B')
                    {
                        playerVectorValues[currentPosition] += "-";
                    }
                }
            }

            for (int i = 0; i < playerVectorValuesInt.Length; i++)
            {
                playerVectorValuesInt[i] = int.Parse(playerVectorValues[i]);
            }

            Vector3 position = new Vector3(playerVectorValuesInt[0], playerVectorValuesInt[1], playerVectorValuesInt[2]);
            playerData.playerPosition = position;

            spawnTeleporter.WorldToTeleportTo = playerData.worldId;
            spawnTeleporterPlace.transform.position = playerData.playerPosition;
            #endregion

            #region Map Goal Marker
            char[] mapChars = hexaStrings[4].ToCharArray();
            string[] mapPositionString = new string[2];
            int[] mapPositionInt = new int[2];
            int currentMapPosition = -1;

            foreach (char c in mapChars)
            {
                if (int.TryParse(c.ToString(), out int number))
                {
                    mapPositionString[currentMapPosition] += number.ToString();
                }
                else
                {
                    currentMapPosition++;
                    if (c == 'B')
                    {
                        mapPositionString[currentMapPosition] += "-";
                    }
                }
            }

            for (int i = 0; i < mapPositionString.Length; i++)
            {
                mapPositionInt[i] = int.Parse(mapPositionString[i]);
            }

            Vector2 goalMarkerPosition = new Vector2(mapPositionInt[0], mapPositionInt[1]);
            mapGoalMarker.position = goalMarkerPosition;
            #endregion

            #region Locations Been To
            List<MapMarker> mapMarkers = new List<MapMarker>();

            foreach (World world in FindObjectOfType<WorldManager>().Worlds)
            {
                if (world.markerObject != null)
                {
                    mapMarkers.Add(world.markerObject.GetComponent<MapMarker>());
                }
            }

            bool[] hasVisitedBools = new bool[mapMarkers.Count];
            hasVisitedBools = BinaryHexaConverter.ConvertHexaToBinary(hexaStrings[5], hasVisitedBools.Length);

            for (int i = 0; i < hasVisitedBools.Length; i++)
            {
                mapMarkers[i].HasVisited = hasVisitedBools[i];
            }
            #endregion

            #region Trigger On Touch Used
            bool[] activeTouchTriggers = new bool[triggerOnTouch.Length];
            activeTouchTriggers = BinaryHexaConverter.ConvertHexaToBinary(hexaStrings[6], activeTouchTriggers.Length);

            for (int i = 0; i < activeTouchTriggers.Length; i++)
            {
                triggerOnTouch[i].enabled = activeTouchTriggers[i];
            }
            #endregion
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
    public bool value;

    public PlayerBool(string name, bool value)
    {
        this.name = name;
        this.value = value;
    }
}