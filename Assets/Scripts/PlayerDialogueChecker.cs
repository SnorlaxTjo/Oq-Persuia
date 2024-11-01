using UnityEngine;

public class PlayerDialogueChecker : MonoBehaviour
{
    bool isInteractable;
    Dialogue currentDialogue = null;
    int currentDialogueIndex = 0;

    UIManager uiManager;
    PlayerController playerController;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        Dialogue();
    }

    void Dialogue()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            if (!uiManager.IsDisplayingSymbols)
            {
                if (currentDialogueIndex < currentDialogue.DialogueLines.Length)
                {
                    if (currentDialogue.HasSpoken)
                    {
                        currentDialogueIndex = currentDialogue.DialogueLines.Length - 1;
                        uiManager.ShowDialogue(currentDialogue.DialogueLines[currentDialogueIndex].dialogueLine);
                        currentDialogueIndex++;
                    }
                    else
                    {
                        uiManager.ShowDialogue(currentDialogue.DialogueLines[currentDialogueIndex].dialogueLine);

                        currentDialogueIndex++;
                    }                  
                }
                else
                {
                    uiManager.CloseDialogue();
                    if (!currentDialogue.HasSpoken) currentDialogue.WhatToDoUponFinishedTalking?.Invoke();

                    currentDialogue.HasSpoken = true;
                    currentDialogueIndex = 0;
                }
            }
            else
            {
                uiManager.QuickCompleteDialogue(currentDialogue.DialogueLines[currentDialogueIndex - 1].dialogueLine);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
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
            uiManager.CloseDialogue();

            isInteractable = false;

            currentDialogue = null;
            currentDialogueIndex = 0;
        }
    }
}
