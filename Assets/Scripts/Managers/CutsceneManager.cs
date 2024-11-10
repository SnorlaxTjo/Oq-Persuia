using System;
using TMPro;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] Cutscene[] cutscenes;
    [SerializeField] TextMeshProUGUI cutsceneText;

    [SerializeField] Animator animator;

    public void StartCutscene(int cutsceneToStart)
    {
        animator.SetInteger("AnimationID", cutsceneToStart);

        foreach (GameObject world in cutscenes[cutsceneToStart].extraActiveWorlds)
        {
            world.SetActive(true);
        }
    }

    public void ChangeText(string text)
    {
        cutsceneText.text = text;
    }

    public void ResetAnimationId()
    {
        animator.SetInteger("AnimationID", -1);
    }
}

[Serializable]
public struct Cutscene
{
    public string name;
    public GameObject[] extraActiveWorlds;
}