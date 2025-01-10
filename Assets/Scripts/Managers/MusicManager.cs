using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioClip standardMusicClip;

    bool isFadingMusic;
    bool isFadingDown;
    float standardMusicVolume;
    float currentMusicVolume;
    float maxMusicVolume;

    AudioSource audioSource;

    public float MaxMusicVolume { get { return maxMusicVolume; } }

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
        ChangeStandardVolume(Options.instance.CurrentMusicVolume);
    }

    private void Update()
    {
        if (isFadingMusic)
        {
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

        if (audioSource != null && audioSource.volume > maxMusicVolume)
        {
            audioSource.volume = maxMusicVolume;
        }
    }

    public void ChangeStandardVolume(int volume)
    {
        maxMusicVolume = (float)volume / 100f;
        audioSource.volume = maxMusicVolume;
    }

    public void ChangeMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void ChangeMusicWithFade(AudioClip clip)
    {
        standardMusicVolume = audioSource.volume;
        currentMusicVolume = standardMusicVolume;
        StartCoroutine(FullMusicFadeRoutine(clip));
    }

    public void FadeMusic(bool fadeOut)
    {
        if (fadeOut)
        {
            standardMusicVolume = audioSource.volume;
            currentMusicVolume = standardMusicVolume;
            StartCoroutine(MusicFadeOutRoutine());
        }
        else
        {
            StartCoroutine(MusicFadeInRoutine());
        }
    }

    IEnumerator FullMusicFadeRoutine(AudioClip clip)
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
        standardMusicVolume = maxMusicVolume;

        yield return new WaitForSeconds(1f);

        currentMusicVolume = maxMusicVolume;
        isFadingMusic = false;
    }

    IEnumerator MusicFadeOutRoutine()
    {
        isFadingDown = true;
        isFadingMusic = true;

        yield return new WaitForSeconds(1f);

        currentMusicVolume = 0f;
        isFadingMusic = false;
    }

    IEnumerator MusicFadeInRoutine()
    {
        isFadingDown = false;
        isFadingMusic = true;

        yield return new WaitForSeconds(1f);

        currentMusicVolume = maxMusicVolume;
        isFadingMusic = false;
    }

    public void PlayStandardMusic()
    {
        if (audioSource.clip != standardMusicClip)
        {
            ChangeMusicWithFade(standardMusicClip);
        }
    }
}
