using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioClip standardMusicClip;

    bool isFadingMusic;
    bool isFadingDown;
    float standardMusicVolume;
    float currentMusicVolume;

    AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Debug.Log("a");
        ChangeStandardVolume(Options.instance.CurrentMusicVolume);
    }

    private void Update()
    {
        if (!isFadingMusic) { return; }

        if (isFadingDown)
        {
            currentMusicVolume -= Time.deltaTime * standardMusicVolume;
        }
        else
        {
            currentMusicVolume += Time.deltaTime * standardMusicVolume;
        }

        audioSource.volume = currentMusicVolume;
    }

    public void ChangeStandardVolume(int volume)
    {
        standardMusicVolume = (float)volume / 100f;
        audioSource.volume = standardMusicVolume;
    }

    public void ChangeMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void ChangeMusicWithFade(AudioClip clip, float endVolume)
    {
        standardMusicVolume = audioSource.volume;
        currentMusicVolume = standardMusicVolume;
        StartCoroutine(MusicFadeRoutine(clip, endVolume));
    }

    IEnumerator MusicFadeRoutine(AudioClip clip, float endVolume)
    {
        isFadingDown = true;
        isFadingMusic = true;

        yield return new WaitForSeconds(1f);

        currentMusicVolume = 0f;
        isFadingMusic = false;
        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitForSeconds(0.5f);

        isFadingDown = false;
        isFadingMusic = true;
        standardMusicVolume = endVolume;

        yield return new WaitForSeconds(1f);

        currentMusicVolume = endVolume;
        isFadingMusic = false;
    }

    public void PlayStandardMusic()
    {
        if (audioSource.clip != standardMusicClip)
        {
            ChangeMusicWithFade(standardMusicClip, 1);
        }
    }
}
