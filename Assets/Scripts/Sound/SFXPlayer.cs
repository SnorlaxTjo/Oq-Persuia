using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    AudioSource source;

    public void PlayClip(AudioClip clipToPlay)
    {
        source = GetComponent<AudioSource>();

        source.clip = clipToPlay;
        StartCoroutine(PlayClipRoutine());
    }

    IEnumerator PlayClipRoutine()
    {
        source.Play();

        yield return new WaitForSeconds(source.clip.length);

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
