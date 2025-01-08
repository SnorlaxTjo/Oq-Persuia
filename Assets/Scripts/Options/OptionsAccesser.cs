using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsAccesser : MonoBehaviour
{
    [SerializeField] GameObject displayOnLeavingOptions;

    public void ShowOptions(bool show)
    {       
        Options options = FindObjectOfType<Options>(true);
        options.gameObject.SetActive(show);
        options.SaveOldOptions();
    }

    public void QuitOptions()
    {
        ShowOptions(false);
        displayOnLeavingOptions.SetActive(true);
    }
}
