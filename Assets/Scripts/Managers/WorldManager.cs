using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] World[] worlds;

    Vector3[] localCameraLimits = new Vector3[2];

    CameraManager cameraManager;

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    public Vector3[] LocalCameraLimits { get { return localCameraLimits; } }

    public void ChangeWorld(int worldToChangeTo, int teleportPosition)
    {
        StartCoroutine(ChangeWorldRoutine(worldToChangeTo, teleportPosition));
    }

    IEnumerator ChangeWorldRoutine(int world, int teleportPosition)
    {
        player.GetComponent<PlayerController>().MoveBlock = true;
        player.GetComponent<CharacterController>().enabled = false;

        yield return new WaitForEndOfFrame();

        World setWorld = worlds[world];

        localCameraLimits[0] = new Vector3(setWorld.world.transform.position.x + setWorld.lowerCameraLimit.x, setWorld.world.transform.position.y + setWorld.lowerCameraLimit.y, setWorld.world.transform.position.z + setWorld.lowerCameraLimit.z);
        localCameraLimits[1] = new Vector3(setWorld.world.transform.position.x + setWorld.upperCameraLimit.x, setWorld.world.transform.position.y + setWorld.upperCameraLimit.y, setWorld.world.transform.position.z + setWorld.upperCameraLimit.z);

        foreach (World _world in worlds)
        {
            _world.world.SetActive(false);
        }
        setWorld.world.SetActive(true);

        cameraManager.ChangeCameraLimits(localCameraLimits[0], localCameraLimits[1]);
        player.transform.position = setWorld.teleportPlaces[teleportPosition].position;

        yield return new WaitForEndOfFrame();

        player.GetComponent<CharacterController>().enabled = true;
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