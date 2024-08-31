using UnityEngine;

public class PlayerDialogueChecker : MonoBehaviour
{
    bool isInteractable;
    Dialogue currentDialogue = null;
    int currentDialogueIndex = 0;

    UIManager uiManager;

    private void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();

        Debug.Log(uiManager.gameObject.name);
    }

    private void Update()
    {
        Debug.Log("aa");

        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                uiManager.ShowDialogue("This is a test Dialogue!");
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("a");

        if (other.gameObject.CompareTag("Dialogue"))
        {
            isInteractable = true;

            currentDialogue = other.gameObject.GetComponent<Dialogue>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Dialogue"))
        {
            isInteractable = false;

            currentDialogue = null;
            currentDialogueIndex = 0;
        }
    }
}
