using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] GameObject sfxPlayer;

    [Space]

    [SerializeField] AudioClip[] sfxClips;

    public void CreateSFX(int sfxToPlay)
    {
        GameObject sfxObject = Instantiate(sfxPlayer);
        SFXPlayer sfx = sfxObject.GetComponent<SFXPlayer>();

        if (sfxToPlay >= sfxClips.Length || sfxToPlay < 0)
        {
            Debug.LogError("ErrorCode: Bönor. \n Invalid SFX ID");
        }
        else
        {
            sfx.PlayClip(sfxClips[sfxToPlay]);
        }
    }
}
