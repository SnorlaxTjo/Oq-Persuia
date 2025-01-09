using System;
using System.Collections;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] World[] worlds;
    [SerializeField] float timeToHaveBlackScreen;

    Vector3[] localCameraLimits = new Vector3[2];

    bool turnOffCameraCollisions;

    bool changeCameraPosition;
    Vector3 cameraPosition;
    float cameraRotation;

    int currentWorld;

    CameraManager cameraManager;
    UIManager uiManager;
    CutsceneManager cutsceneManager;

    Bow bow;

    public World[] Worlds { get { return worlds; } }
    public Vector3[] LocalCameraLimits { get { return localCameraLimits; } }
    public int CurrentWorld { get { return currentWorld; } }

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        uiManager = FindObjectOfType<UIManager>();
        cutsceneManager = FindObjectOfType<CutsceneManager>();

        bow = FindObjectOfType<Bow>();
    }  

    public void ChangeWorld(int worldToChangeTo, int teleportPosition, bool startCutscene = false, int cutscene = 0)
    {
        StartCoroutine(ChangeWorldRoutine(worldToChangeTo, teleportPosition, startCutscene, cutscene));
    }

    IEnumerator ChangeWorldRoutine(int world, int teleportPosition, bool startCutscene, int cutscene)
    {
        World setWorld = worlds[world];
        currentWorld = world;

        WorldInfo setWorldInfo = setWorld.world.GetComponent<WorldInfo>();

        player.GetComponent<PlayerController>().CompleteMoveBlock = true;

        uiManager.SetTransition(true);
        uiManager.MapBlock = true;

        if (setWorldInfo.UseCustomMusic)
        {
            MusicManager.instance.ChangeMusicWithFade(setWorldInfo.CustomMusic);
        }
        else
        {
            MusicManager.instance.PlayStandardMusic();
        }

        yield return new WaitForSeconds(1);

        Vector3 lowerCameraLimit = setWorldInfo.LowerCameraLimit;
        Vector3 upperCameraLimit = setWorldInfo.UpperCameraLimit;

        localCameraLimits[0] = new Vector3(lowerCameraLimit.x, 0, lowerCameraLimit.z);
        localCameraLimits[1] = new Vector3(upperCameraLimit.x, 0, upperCameraLimit.z);

        turnOffCameraCollisions = setWorldInfo.TurnOffCameraCollisions;

        changeCameraPosition = setWorldInfo.UseCustomCameraPosition;      

        foreach (World _world in worlds)
        {
            _world.world.SetActive(false);
        }
        setWorld.world.SetActive(true);

        cameraManager.ChangeCameraLimits(localCameraLimits[0], localCameraLimits[1]);
        if (teleportPosition != -1)
        {
            player.GetComponent<PlayerController>().Teleport(setWorld.teleportPlaces[teleportPosition].position);
        }
        else
        {
            player.GetComponent<PlayerController>().Teleport(ProgressKeeper.instance.SpawnTeleporterPlace.transform.position);
        }

        cameraManager.CanCheckForCollision = !turnOffCameraCollisions;

        if (changeCameraPosition)
        {
            cameraPosition = setWorldInfo.CameraPlayerOffset;
            cameraRotation = setWorldInfo.CameraRotation;

            cameraManager.ChangeCameraPosition(cameraPosition, cameraRotation);
        }
        else
        {
            cameraManager.ResetCameraPosition();
        }

        bow.CanShoot = setWorldInfo.Bow;

        if (startCutscene)
        {
            cutsceneManager.StartCutscene(cutscene);
        }

        uiManager.SetMapPlayerMarker(setWorld.markerPosition);
        if (setWorld.markerObject != null)
        {
            setWorld.markerObject.GetComponent<MapMarker>().HasVisited = true;
        }    

        FindObjectOfType<FastTravel>().EnableTravel(setWorldInfo.CanFastTravel);
        ProgressKeeper.instance.EnableSave(setWorldInfo.CanSave);

        yield return new WaitForSeconds(timeToHaveBlackScreen);

        uiManager.SetTransition(false);
        uiManager.MapBlock = false;

        player.GetComponent<PlayerController>().CompleteMoveBlock = false;
    }
}

[Serializable]
public struct World
{
    public string name;
    public GameObject world;
    public Transform[] teleportPlaces;
    public GameObject markerObject;
    public Vector2 markerPosition;
}