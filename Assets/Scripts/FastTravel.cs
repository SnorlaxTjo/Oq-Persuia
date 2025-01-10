using UnityEngine;
using UnityEngine.UI;

public class FastTravel : MonoBehaviour
{
    [SerializeField] Button fastTravelButton;

    bool hasGottenFastTravel;
    bool canTravel;

    int worldToTeleportTo;

    WorldManager worldManager;
    UIManager uiManager;

    public bool HasGottenFastTravel { set { hasGottenFastTravel = value; } }
    public bool CanTravel { get { return canTravel; } }
    public int WorldToTeleportTo { get { return worldToTeleportTo; } set { worldToTeleportTo = value; } }

    private void Start()
    {
        worldManager = FindObjectOfType<WorldManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void EnableTravel(bool enable)
    {
        canTravel = enable;
        fastTravelButton.interactable = enable && hasGottenFastTravel;
    }

    public void Travel()
    {
        if (worldToTeleportTo == 0)
        {
            Debug.LogWarning("The world ID for the world you wanted to teleport to was set to 0, you are sent to the airport");
        }

        uiManager.HideMiniMenu();
        foreach (MapMarker marker in FindObjectsOfType<MapMarker>())
        {
            marker.ResetMarker();
        }

        worldManager.ChangeWorld(worldToTeleportTo, 0);
        uiManager.DisplayMap(false);
    }
}
