using System;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    [SerializeField] bool displayName = true;
    [SerializeField] DialogueLine[] dialogueLines2;
    [SerializeField] UnityEvent whatToDoUponFinishedTalking;

    bool hasSpoken;

    public bool DisplayName { get { return displayName; } }
    public DialogueLine[] DialogueLines { get { return dialogueLines2; } }
    public UnityEvent WhatToDoUponFinishedTalking { get { return whatToDoUponFinishedTalking; } }
    public bool HasSpoken { get {  return hasSpoken; } set { hasSpoken = value; } }
}

[Serializable]
public struct DialogueLine
{
    public string npcName;
    public string dialogueLine;
}
