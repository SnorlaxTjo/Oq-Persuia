using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    [SerializeField] string[] dialogueLines;
    [SerializeField] UnityEvent whatToDoUponFinishedTalking;

    bool hasSpoken;
    
    public string[] DialogueLines { get { return dialogueLines; } }
    public UnityEvent WhatToDoUponFinishedTalking { get { return whatToDoUponFinishedTalking; } }
    public bool HasSpoken { get {  return hasSpoken; } set { hasSpoken = value; } }
}
