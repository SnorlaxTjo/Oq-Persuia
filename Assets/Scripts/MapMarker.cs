using UnityEngine;
using UnityEngine.UI;

public enum MapMarkerType
{
    City,
    POI
}

public class MapMarker : MonoBehaviour
{
    [SerializeField] MapMarkerType markerType;
    [SerializeField] Sprite placeImage;
    [SerializeField] TextAsset placeInfo;

    [Space]
    [SerializeField] int worldId;

    string title;
    string description;
    string locations;

    bool isActivated;
    bool hasClicked;
    bool elseClicked;

    bool hasVisited;

    Image markerImage;
    UIManager uiManager;

    public bool ElseClicked { set { elseClicked = value; } }
    public bool HasVisited { get { return hasVisited; } set { hasVisited = value; } }

    private void Start()
    {
        markerImage = GetComponent<Image>();
        uiManager = FindObjectOfType<UIManager>();

        SortTextFile();
    }

    private void Update()
    {
        if (!isActivated) { return; }

        if (Input.GetMouseButtonUp(0))
        {
            hasClicked = !hasClicked;

            foreach(MapMarker mapMarker in FindObjectsOfType<MapMarker>())
            {
                mapMarker.ElseClicked = hasClicked;
            }

            elseClicked = false;

            if (!hasClicked)
            {
                uiManager.ClearCityInfo();

                markerImage.color = new Color(1f, 1f, 1f);

                isActivated = false;
            }
            else
            {
                ShowTinyMenu();
            }          
        }
    }

    public void ResetMarker()
    {
        uiManager.ClearCityInfo();

        markerImage.color = new Color(1f, 1f, 1f);

        isActivated = false;
        hasClicked = false;
        elseClicked = false;
    }

    void ShowTinyMenu()
    {
        uiManager.ShowMiniMenu(markerType, (Vector2)gameObject.GetComponent<Image>().rectTransform.position, gameObject.name);
        FastTravel fastTravel = FindObjectOfType<FastTravel>();
        fastTravel.EnableTravel(hasVisited);
        fastTravel.WorldToTeleportTo = worldId;
    }

    void SortTextFile()
    {
        string[] notes = placeInfo.text.Split("\r\n\r\n");

        switch (notes[0])
        {
            case "CITY":
                title = notes[1];
                description = notes[2];
                locations = "Locations:\n" + notes[3];
                break;
            case "POI":
                title = notes[1];
                description = notes[2];
                locations = string.Empty;
                break;
            default:
                Debug.LogWarning(placeInfo.name + " does not have city or POI correctly");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (elseClicked) { return; }

        if (other.gameObject.CompareTag("MouseFollower"))
        {
            uiManager.ShowCityInfo(placeImage, title, description, locations);

            markerImage.color = new Color(0.7f, 0.7f, 0.7f);

            isActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasClicked || elseClicked) { return; }

        if (other.gameObject.CompareTag("MouseFollower"))
        {
            uiManager.ClearCityInfo();

            markerImage.color = new Color(1f, 1f, 1f);

            isActivated = false;
        }
    }
}
