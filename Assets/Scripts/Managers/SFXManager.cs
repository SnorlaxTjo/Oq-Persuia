using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] GameObject sfxPlayer;

    [Space]

    [SerializeField] AudioClip[] sfxClips;
    [SerializeField] int[] musicClips;

    float sfxVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SetVolume(Options.instance.CurrentSfxVolume);
    }

    public void SetVolume(int volume)
    {
        sfxVolume = (float)volume / 100f;
    }

    public void CreateSFX(int sfxToPlay)
    {
        GameObject sfxObject = Instantiate(sfxPlayer);
        SFXPlayer sfx = sfxObject.GetComponent<SFXPlayer>();

        if (sfxToPlay >= sfxClips.Length || sfxToPlay < 0)
        {
            Debug.LogWarning("Invalid SFX ID");
        }
        else
        {
            bool isMusic = false;
            foreach (int music in musicClips)
            {
                if (music ==  sfxToPlay)
                {
                    isMusic = true;
                }
            }

            if (isMusic)
            {
                sfxObject.GetComponent<AudioSource>().volume = MusicManager.instance.MaxMusicVolume;
            }
            else
            {
                sfxObject.GetComponent<AudioSource>().volume = sfxVolume;
            }
            
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
