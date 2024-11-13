using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] GameObject dialogueBackground;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] GameObject nameBackground;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] float timeBetweenEachSymbol = 0.05f;

    [Header("Map")]
    [SerializeField] GameObject mapMenu;
    [SerializeField] Sprite standardSprite;
    [SerializeField] Image placeImage;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI locationsText;
    [SerializeField] Image playerMarker;
    [SerializeField] Image goalMarker;
    [SerializeField] GameObject cityMenu;
    [SerializeField] GameObject poiMenu;

    [Header("Transition")]
    [SerializeField] Animator transitionAnimator;

    [Header("Player Health Bar")]
    [SerializeField] Image playerHealthBar;
    [SerializeField] Image playerHealthBarBackground;

    [Header("Boss Health Bar")]
    [SerializeField] Image bossHealthBar;
    [SerializeField] Image bossHealthBarBackground;

    #region Dialogue Variables
    bool isDisplayingSymbols;
    char[] dialogueChars;
    int currentChar;
    float timeUntilNextSymbol;
    #endregion

    #region Map Variables
    bool hasGottenMap;
    bool isShowingMap;
    #endregion

    #region Health Bar Variables
    float fullPlayerHealthBarLength;
    float fullBossHealthBarLength;

    bool fadingPlayerHealthBar;
    float playerHealthBarAlpha;
    bool playerHealthBarVisible;

    bool fadingBossHealthBar;
    float bossHealthBarAlpha;
    bool bossHealthBarVisible;
    #endregion

    PlayerController playerController;
    PlayerHealth playerHealth;

    public bool IsDisplayingSymbols { get { return isDisplayingSymbols; } }
    public bool HasGottenMap { set { hasGottenMap = value; } }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerHealth = FindObjectOfType<PlayerHealth>();

        fullPlayerHealthBarLength = playerHealthBar.rectTransform.rect.width;
        fullBossHealthBarLength = bossHealthBar.rectTransform.rect.width;
    }

    private void Update()
    {
        DisplayDialogue();
        ToggleMap();
        SetHealthBar();
        FadeHealthBars();
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
                if (dialogueChars[currentChar - 1] != ' ')
                {
                    timeUntilNextSymbol += timeBetweenEachSymbol;
                }               
            }
            if (currentChar >= dialogueChars.Length)
            {
                isDisplayingSymbols = false;
            }

            timeUntilNextSymbol -= Time.deltaTime;
        }
    }

    public void ShowDialogue(string dialogue, bool showName, string name)
    {
        dialogueBackground.SetActive(true);

        dialogueText.text = string.Empty;
        currentChar = 0;
        dialogueChars = dialogue.ToCharArray();
        isDisplayingSymbols = true;

        if (showName)
        {
            nameBackground.SetActive(true);
            nameText.text = name;
        }

        playerController.CompleteMoveBlock = true;
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

        nameBackground.SetActive(false);
        nameText.text = string.Empty;

        playerController.CompleteMoveBlock = false;
    }
    #endregion

    #region Map
    void ToggleMap()
    {
        if (!hasGottenMap) { return; }

        if (Input.GetKeyDown(KeyCode.M))
        {
            isShowingMap = !isShowingMap;
            mapMenu.SetActive(isShowingMap);
            playerController.CompleteMoveBlock = isShowingMap;
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
        HideMiniMenu();

        placeImage.sprite = standardSprite;
        titleText.text = string.Empty;
        descriptionText.text = string.Empty;
        locationsText.text = string.Empty;       
    }

    public void SetMapPlayerMarker(Vector2 position)
    {
        playerMarker.rectTransform.position = position + new Vector2(960, 540);
    }

    public void SetMapGoalMarker(Vector2 position)
    {
        goalMarker.rectTransform.position = position + new Vector2(960, 540);
    }

    public void ShowMiniMenu(MapMarkerType markerType, Vector2 position)
    {
        switch (markerType)
        {
            case MapMarkerType.City:
                cityMenu.SetActive(true);
                cityMenu.GetComponent<RectTransform>().position = position;
                break;
            case MapMarkerType.POI:
                poiMenu.SetActive(true);
                poiMenu.GetComponent<RectTransform>().position = position;
                break;
        }
    }

    public void HideMiniMenu()
    {
        cityMenu.SetActive(false);
        poiMenu.SetActive(false);
    }
    #endregion

    #region Transition
    public void SetTransition(bool whatToSet)
    {
        transitionAnimator.SetBool("Transition", whatToSet);
    }  
    #endregion

    #region Health Bars

    void SetHealthBar()
    {
        int maxHealth = playerHealth.MaxHealth;
        int currentHealth = playerHealth.CurrentHealth;

        float healthPercentage = (float)currentHealth / (float)maxHealth;
        float healthBarLength = fullPlayerHealthBarLength * healthPercentage;

        playerHealthBar.rectTransform.sizeDelta = new Vector2(healthBarLength, playerHealthBar.rectTransform.rect.height);
    }

    public void SetBossHealthBar(int bossHealth, int maxHealth)
    {
        float healthPercentage = (float)bossHealth / (float)maxHealth;
        float healthBarLength = fullBossHealthBarLength * healthPercentage;

        bossHealthBar.rectTransform.sizeDelta = new Vector2(healthBarLength, bossHealthBar.rectTransform.rect.height);
    }

    public void ActivateBossHealthBar()
    {
        fadingBossHealthBar = true;
    }

    void FadeHealthBars()
    {
        if (fadingPlayerHealthBar)
        {

        }

        if (fadingBossHealthBar)
        {
            if (!bossHealthBarVisible)
            {
                bossHealthBarAlpha += Time.deltaTime;

                if (bossHealthBarAlpha >= 1f)
                {
                    bossHealthBarAlpha = 1f;
                    bossHealthBarVisible = true;
                    fadingBossHealthBar = false;
                }

                bossHealthBar.color = new Color(1f, 0f, 0f, bossHealthBarAlpha);
                bossHealthBarBackground.color = new Color(0.5f, 0f, 0f, bossHealthBarAlpha);
            }
        }
    }

    #endregion
}
