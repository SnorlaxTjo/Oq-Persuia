using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMarker : MonoBehaviour
{
    [SerializeField] Sprite placeImage;
    [SerializeField] TextAsset placeInfo;

    string title;
    string description;
    string locations;

    Image markerImage;
    UIManager uiManager;

    private void Start()
    {
        markerImage = GetComponent<Image>();
        uiManager = FindObjectOfType<UIManager>();

        SortTextFile();
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
        if (other.gameObject.CompareTag("MouseFollower"))
        {
            uiManager.ShowCityInfo(placeImage, title, description, locations);

            markerImage.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MouseFollower"))
        {
            uiManager.ClearCityInfo();

            markerImage.color = new Color(1f, 1f, 1f);
        }
    }
}
