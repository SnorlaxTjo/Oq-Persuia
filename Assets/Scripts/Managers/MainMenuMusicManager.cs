using UnityEngine;

public class MainMenuMusicManager : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        SetVolume();
    }

    public void SetVolume()
    {
        while (Options.instance == null)
        {
            return;
        }

        audioSource.volume = (float)Options.instance.CurrentMusicVolume / 100f;
    }
}
