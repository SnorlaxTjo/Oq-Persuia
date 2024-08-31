using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] string[] dialogueLines;
    
    public string[] DialogueLines { get { return dialogueLines; } }
}
