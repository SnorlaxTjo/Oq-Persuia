using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] World[] worlds;
    [SerializeField] float timeToHaveBlackScreen;

    Vector3[] localCameraLimits = new Vector3[2];

    CameraManager cameraManager;
    UIManager uiManager;

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public Vector3[] LocalCameraLimits { get { return localCameraLimits; } }

    public void ChangeWorld(int worldToChangeTo, int teleportPosition)
    {
        StartCoroutine(ChangeWorldRoutine(worldToChangeTo, teleportPosition));
    }

    IEnumerator ChangeWorldRoutine(int world, int teleportPosition)
    {
        player.GetComponent<PlayerController>().MoveBlock = true;

        uiManager.SetTransition(true);

        yield return new WaitForSeconds(1);

        World setWorld = worlds[world];

        Vector3 lowerCameraLimit = setWorld.world.GetComponent<WorldInfo>().LowerCameraLimit;
        Vector3 upperCameraLimit = setWorld.world.GetComponent<WorldInfo>().UpperCameraLimit;

        localCameraLimits[0] = new Vector3(lowerCameraLimit.x, 0, lowerCameraLimit.z);
        localCameraLimits[1] = new Vector3(upperCameraLimit.x, 0, upperCameraLimit.z);

        foreach (World _world in worlds)
        {
            _world.world.SetActive(false);
        }
        setWorld.world.SetActive(true);

        cameraManager.ChangeCameraLimits(localCameraLimits[0], localCameraLimits[1]);
        player.transform.position = setWorld.teleportPlaces[teleportPosition].position;

        yield return new WaitForSeconds(timeToHaveBlackScreen);

        uiManager.SetTransition(false);

        player.GetComponent<PlayerController>().MoveBlock = false;
    }
}

[Serializable]
public struct World
{
    public string name;
    public GameObject world;
    public Transform[] teleportPlaces;
    public Vector3 lowerCameraLimit;
    public Vector3 upperCameraLimit;
}