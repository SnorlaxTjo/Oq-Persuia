using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class UIManager : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] GameObject dialogueBackground;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] float timeBetweenEachSymbol = 0.05f;

    [Header("Map")]
    [SerializeField] GameObject mapMenu;
    [SerializeField] Sprite standardSprite;
    [SerializeField] Image placeImage;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI locationsText;

    #region Dialogue Variables
    bool isDisplayingSymbols;
    char[] dialogueChars;
    int currentChar;
    float timeUntilNextSymbol;
    #endregion

    #region Map Variables
    bool isShowingMap;
    #endregion

    PlayerController playerController;

    public bool IsDisplayingSymbols { get { return isDisplayingSymbols; } }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        DisplayDialogue();
        ToggleMap();
    }

    #region Dialogue
    void DisplayDialogue()
    {
        if (isDisplayingSymbols)
        {
            if (timeUntilNextSymbol <= 0 && currentChar < dialogueChars.Length)
            {
                dialogueText.text += dialogueChars[currentChar];

                currentChar++;
                timeUntilNextSymbol += timeBetweenEachSymbol;
            }
            if (currentChar >= dialogueChars.Length)
            {
                isDisplayingSymbols = false;
            }

            timeUntilNextSymbol -= Time.deltaTime;
        }
    }

    public void ShowDialogue(string dialogue)
    {
        dialogueBackground.SetActive(true);

        dialogueText.text = string.Empty;
        currentChar = 0;
        dialogueChars = dialogue.ToCharArray();
        isDisplayingSymbols = true;

        playerController.MoveBlock = true;
    }

    public void QuickCompleteDialogue(string dialogue)
    {
        isDisplayingSymbols = false;

        dialogueText.text = dialogue;
        currentChar = dialogueChars.Length;
    }

    public void CloseDialogue()
    {
        dialogueBackground.SetActive(false);
        isDisplayingSymbols = false;
        currentChar = 0;
        dialogueText.text = string.Empty;

        playerController.MoveBlock = false;
    }
    #endregion

    #region Map
    void ToggleMap()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isShowingMap = !isShowingMap;
            mapMenu.SetActive(isShowingMap);
            playerController.MoveBlock = isShowingMap;
        }
    }

    public void ShowCityInfo(Sprite sprite, string title, string description, string locations)
    {
        placeImage.sprite = sprite;
        titleText.text = title;
        descriptionText.text = description;
        locationsText.text = locations;
    }

    public void ClearCityInfo()
    {
        placeImage.sprite = standardSprite;
        titleText.text = string.Empty;
        descriptionText.text = string.Empty;
        locationsText.text = string.Empty;
    }
    #endregion
}
