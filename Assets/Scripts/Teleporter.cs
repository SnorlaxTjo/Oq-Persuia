using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Teleporter : MonoBehaviour
{
    [SerializeField] int worldToTeleportTo;
    [SerializeField] int teleporterPlaceToTeleportTo;
    [SerializeField] bool triggerCutscene;
    [SerializeField] int cutsceneToTrigger;

    [SerializeField] bool playSound = true;
    [SerializeField] int soundToPlay = 1;

    WorldManager worldManager;

    public int WorldToTeleportTo { set { worldToTeleportTo = value; } }

    private void Start()
    {
        worldManager = FindObjectOfType<WorldManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            worldManager.ChangeWorld(worldToTeleportTo, teleporterPlaceToTeleportTo, triggerCutscene, cutsceneToTrigger);
            if (playSound)
            {
                SFXManager.instance.CreateSFX(soundToPlay);
            }
        }
    }
}
