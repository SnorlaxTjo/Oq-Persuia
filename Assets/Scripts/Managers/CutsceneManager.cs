using System;
using TMPro;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] Cutscene[] cutscenes;
    [SerializeField] TextMeshProUGUI cutsceneText;
    [SerializeField] GameObject[] cameras;

    [SerializeField] Animator animator;

    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void StartCutscene(int cutsceneToStart)
    {
        animator.enabled = true;
        animator.SetInteger("AnimationID", cutsceneToStart);

        foreach (GameObject world in cutscenes[cutsceneToStart].extraActiveWorlds)
        {
            world.SetActive(true);
        }
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);

        playerController.CompleteMoveBlock = true;
    }

    public void ChangeText(string text)
    {
        cutsceneText.text = text;
    }

    public void ResetAnimationId()
    {
        animator.SetInteger("AnimationID", -1);
        cameras[0].SetActive(true);
        cameras[1].SetActive(false);

        playerController.CompleteMoveBlock = false;
        animator.enabled = false;
    }

    public void ResetCamera()
    {
        cameras[0].SetActive(true);
        cameras[1].SetActive(false);
    }

    public void SetCutsceneCameraActive()
    {
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);
    }

    public void TriggerSFX(int sfxToPlay)
    {
        SFXManager.instance.CreateSFX(sfxToPlay);
    }

    public void FadeMusic(int fadeOut)
    {
        MusicManager.instance.FadeMusic(fadeOut == 0);
    }
}

[Serializable]
public struct Cutscene
{
    public string name;
    public GameObject[] extraActiveWorlds;
}