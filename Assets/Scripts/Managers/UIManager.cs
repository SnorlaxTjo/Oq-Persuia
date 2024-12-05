using System.Linq.Expressions;
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
    [SerializeField] TextMeshProUGUI cityText;
    [SerializeField] GameObject poiMenu;
    [SerializeField] TextMeshProUGUI poiText;
    [SerializeField] int[] mapSounds;

    [Header("Transition")]
    [SerializeField] Animator transitionAnimator;

    [Header("Player Health Bar")]
    [SerializeField] Image playerHealthBar;
    [SerializeField] Image playerHealthBarBackground;

    [Header("Boss Health Bar")]
    [SerializeField] Image bossHealthBar;
    [SerializeField] Image bossHealthBarBackground;

    [Header("Phone")]
    [SerializeField] GameObject phoneMenu;

    #region Dialogue Variables
    bool isDisplayingSymbols;
    char[] dialogueChars;
    int currentChar;
    float timeUntilNextSymbol;
    #endregion

    #region Map Variables
    bool hasGottenMap;
    bool isShowingMap;
    bool mapBlock;
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

    #region Phone Variables
    bool hasObtainedPhone;
    #endregion

    PlayerController playerController;
    PlayerHealth playerHealth;

    public bool IsDisplayingSymbols { get { return isDisplayingSymbols; } }
    public bool HasGottenMap { set { hasGottenMap = value; } }
    public bool MapBlock { get { return mapBlock; } set { mapBlock = value; } }
    public bool HasObtainedPhone { set { hasObtainedPhone = value; } }

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
        TogglePhoneMenu();
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
        if (!hasGottenMap || mapBlock) { return; }

        if (Input.GetKeyDown(KeyCode.M))
        {
            DisplayMap(!isShowingMap);
        }
    }

    public void DisplayMap(bool displayMap)
    {
        isShowingMap = displayMap;
        mapMenu.SetActive(isShowingMap);
        playerController.CompleteMoveBlock = isShowingMap;

        SFXManager sfx = FindObjectOfType<SFXManager>();

        if (displayMap)
        {
            sfx.CreateSFX(mapSounds[0]);
        }
        else
        {
            sfx.CreateSFX(mapSounds[1]);
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

    public void SetGoalMarkerX(float positionX)
    {
        goalMarker.rectTransform.position = new Vector2(positionX + 960, goalMarker.rectTransform.position.y);
    }

    public void SetGoalMarkerY(float positionY)
    {
        goalMarker.rectTransform.position = new Vector2(goalMarker.rectTransform.position.x, positionY + 540);
    }

    public void ShowMiniMenu(MapMarkerType markerType, Vector2 position, string name)
    {
        switch (markerType)
        {
            case MapMarkerType.City:
                cityMenu.SetActive(true);
                cityMenu.GetComponent<RectTransform>().position = position;
                cityText.text = name;
                break;
            case MapMarkerType.POI:
                poiMenu.SetActive(true);
                poiMenu.GetComponent<RectTransform>().position = position;
                poiText.text = name;
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
        bool healthBarShouldBeVisible = playerHealth.CurrentHealth < playerHealth.MaxHealth;

        if (!fadingPlayerHealthBar && healthBarShouldBeVisible != playerHealthBarVisible)
        {
            ActivatePlayerHealthBar(healthBarShouldBeVisible);
        }
    }

    public void SetBossHealthBar(int bossHealth, int maxHealth)
    {
        float healthPercentage = (float)bossHealth / (float)maxHealth;
        float healthBarLength = fullBossHealthBarLength * healthPercentage;

        bossHealthBar.rectTransform.sizeDelta = new Vector2(healthBarLength, bossHealthBar.rectTransform.rect.height);
    }

    void ActivatePlayerHealthBar(bool fadeOn)
    {
        playerHealthBarVisible = !fadeOn;
        fadingPlayerHealthBar = true;
    }

    public void ActivateBossHealthBar()
    {
        fadingBossHealthBar = true;
    }

    void FadeHealthBars()
    {
        if (fadingPlayerHealthBar)
        {
            if (!playerHealthBarVisible)
            {
                playerHealthBarAlpha += Time.deltaTime;

                if (playerHealthBarAlpha >= 1f)
                {
                    playerHealthBarAlpha = 1f;
                    playerHealthBarVisible = true;
                    fadingPlayerHealthBar = false;
                }              
            }
            else
            {
                playerHealthBarAlpha -= Time.deltaTime;

                if (playerHealthBarAlpha <= 1f)
                {
                    playerHealthBarAlpha = 0f;
                    playerHealthBarVisible = false;
                    fadingPlayerHealthBar = false;
                }
            }

            playerHealthBar.color = new Color(1f, 0f, 0f, playerHealthBarAlpha);
            playerHealthBarBackground.color = new Color(0.5f, 0f, 0f, playerHealthBarAlpha);
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
            }
            else
            {
                bossHealthBarAlpha -= Time.deltaTime;

                if (bossHealthBarAlpha <= 0f)
                {
                    bossHealthBarAlpha = 0f;
                    bossHealthBarVisible = false;
                    fadingBossHealthBar = false;
                }
            }

            bossHealthBar.color = new Color(1f, 0f, 0f, bossHealthBarAlpha);
            bossHealthBarBackground.color = new Color(0.5f, 0f, 0f, bossHealthBarAlpha);
        }
    }

    #endregion

    #region Phone

    void TogglePhoneMenu()
    {
        if (!hasObtainedPhone) { return; }

        if (Input.GetKeyDown(KeyCode.P))
        {
            phoneMenu.SetActive(!phoneMenu.activeSelf);
        }
    }

    #endregion
}
