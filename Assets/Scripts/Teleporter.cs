using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Teleporter : MonoBehaviour
{
    [SerializeField] int worldToTeleportTo;
    [SerializeField] int teleporterPlaceToTeleportTo;
    [SerializeField] bool turnOffCameraColliderChecker;

    WorldManager worldManager;
    CameraManager cameraManager;

    private void Start()
    {
        worldManager = FindObjectOfType<WorldManager>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            worldManager.ChangeWorld(worldToTeleportTo, teleporterPlaceToTeleportTo);
            cameraManager.CanCheckForCollision = !turnOffCameraColliderChecker;
        }
    }
}
