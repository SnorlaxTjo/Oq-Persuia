using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Teleporter : MonoBehaviour
{
    [SerializeField] int worldToTeleportTo;
    [SerializeField] int teleporterPlaceToTeleportTo;
    [SerializeField] bool triggerCutscene;
    [SerializeField] int cutsceneToTrigger;

    [SerializeField] int soundToPlay = 1;

    WorldManager worldManager;

    private void Start()
    {
        worldManager = FindObjectOfType<WorldManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            worldManager.ChangeWorld(worldToTeleportTo, teleporterPlaceToTeleportTo, triggerCutscene, cutsceneToTrigger);
            FindObjectOfType<SFXManager>().CreateSFX(1);
        }
    }
}
