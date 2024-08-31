using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image dialogueBackground;
    [SerializeField] TextMeshProUGUI dialogueText;

    public void ShowDialogue(string dialogue)
    {
        Debug.Log(dialogue);
    }
}
