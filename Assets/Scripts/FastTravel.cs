using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTravel : MonoBehaviour
{
    [SerializeField] Button fastTravelButton;

    bool hasGottenFastTravel;
    bool canTravel;

    public bool HasGottenFastTravel { set { hasGottenFastTravel = value; } }
    public bool CanTravel { get { return canTravel; } }

    public void EnableTravel(bool enable)
    {
        canTravel = enable;
        fastTravelButton.interactable = enable && hasGottenFastTravel;
    }

    public void Travel()
    {
        Debug.Log("Button Works!!!");
    }
}
