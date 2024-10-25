using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Teleporter : MonoBehaviour
{
    [SerializeField] int worldToTeleportTo;
    [SerializeField] int teleporterPlaceToTeleportTo;

    WorldManager worldManager;

    private void Start()
    {
        worldManager = FindObjectOfType<WorldManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            worldManager.ChangeWorld(worldToTeleportTo, teleporterPlaceToTeleportTo);
        }
    }
}
