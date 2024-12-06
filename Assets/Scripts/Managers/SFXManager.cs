using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] GameObject sfxPlayer;

    [Space]

    [SerializeField] AudioClip[] sfxClips;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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

    public void StopSoundEffect(int sfxToStop)
    {
        foreach (SFXPlayer sfx in FindObjectsOfType<SFXPlayer>())
        {
            if (sfx.GetComponent<AudioSource>().clip == sfxClips[sfxToStop])
            {
                sfx.gameObject.SetActive(false);
                Destroy(sfx.gameObject);
            }
        }
    }
}
