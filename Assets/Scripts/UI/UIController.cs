using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    private int timer;
    private PlayerController playerController;
    public GameObject player;
    private bool playerInSettings;
    public string playerCategory;
    public bool playerMultiBuild;
    public int playerMultiBuildTest;
    private bool playerUsedOnScreenButtonBuild;
    private bool playerUsedOnScreenButtonSettings;
    private int counter = 0;
    #region Value to Text Vars
    public float money;
    //private float moneyPerSec;
    private float researchPoints;
    //private float researchPointsPerSec;
    private float civilians;
    #endregion
    #region Fonts & Colours
    public TMPro.TMP_FontAsset UrbanistMedium;
    public TMPro.TMP_FontAsset UrbanistBold;
    private Color moneyColor;
    private Color researchColor;
    private Color civilianColor;
    private Color positiveColor;
    private Color negativeColor;
    #endregion
    #region In Mode UI's
    public GameObject InHeatMap;
    public GameObject InConfigMode;
    public GameObject InCombatMode;
    #endregion
    #region Animation Objects
    private GameObject MoneyAnimate;
    private GameObject ResearchAnimate;
    private GameObject CivilianAnimate;
    private GameObject BottomAnimate;
    #endregion
    #region fpsCounter vars
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;
    private bool highContrast;
    private bool fpsEnabled;
    public TMPro.TMP_Text fpsCounter;
    #endregion
    #region UI Elements
    public TMPro.TMP_Text moneyUI;
    //public TMPro.TMP_Text moneyPerSecUI;
    public TMPro.TMP_Text researchPointsUI;
    //public TMPro.TMP_Text researchPointsPerSecUI;
    public TMPro.TMP_Text civiliansUI;
    public GameObject buildButton;
    public GameObject researchButton;
    public GameObject modifyButton;
    public GameObject deleteButton;
    public GameObject settingsButton;
    public GameObject buildMenu;
    public TMPro.TMP_Text multiBuild;
    #endregion
    #region Build Mode - Categories
    private bool inIndustryCat;
    private bool inDefenceCat;
    private bool inInfraCat;
    private bool inMiscCat;
    public GameObject industryButton;
    public GameObject defenceButton;
    public GameObject infraButton;
    public GameObject miscButton;
    // menus
    public GameObject industryMenu;
    public GameObject defenceMenu;
    public GameObject infraMenu;
    public GameObject miscMenu;
    #endregion
    #region Settings
    public GameObject settingsMenu;
    public GameObject audioMenu;
    public GameObject graphicsMenu;
    #endregion
    #region Unity & Generic Funcs
    void Start()
    {
        Invoke("StorePlayerController", 0);
        Invoke("IndustryCategoryButton", 1);
        Invoke("SetColours", 1);
    }
    void Update()
    {
        GetPlayerVars();
        SetUIValues();
        FPSCounter();
        CheckCategoryState();
        EnterSettingsMenu();
        LeaveSettingsMenu();
        if(playerController.playerInBuildMode)
        {
            ExitBuildMode();
        }
        else
        {
            EnterBuildMode();
        }
    }
    void StorePlayerController()
    {
        playerController = player.GetComponent<PlayerController>();
    }
    void SetColours()
    {
        moneyColor = new Color(0.388f, 0.709f, 0.203f, 1f);
        researchColor = new Color(0f, 0.478f, 0.929f, 1f);
        civilianColor = new Color(0.929f, 0.603f, 0f, 1f);
        positiveColor = new Color(0.376f, 0.643f, 0.223f, 1f);
        negativeColor = new Color(1f, 0f, 0f, 1f);
    }
    void GetPlayerVars()
    {
        money = playerController.money;
        //moneyPerSec = playerController.moneyPerSec;
        researchPoints = playerController.researchPoints;
        //researchPointsPerSec = playerController.researchPointsPerSec;
        civilians = playerController.civilianAmount;
        playerController.playerMultiBuild = playerMultiBuild;
        playerInSettings = playerController.playerInSettings;
    }
    void SetUIValues()
    {
        #region Money
        if(money >= 1)
        {
            moneyUI.text = money.ToString("#,#");
        }
        else
        {
            moneyUI.text = money.ToString();
        }
        #endregion
        #region Research
        if(researchPoints >= 1)
        {
            researchPointsUI.text = researchPoints.ToString("#,#");
        }
        else
        {
            researchPointsUI.text = researchPoints.ToString();
        }
        #endregion
        #region Civilian
        civiliansUI.text = civilians.ToString();
        if(civilians >= 1)
        {
            civiliansUI.text = civilians.ToString("#,#");
        }
        else
        {
            civiliansUI.text = civilians.ToString();
        }
        #endregion
    }
    static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
		//Author: Isaac Dart, June-13.
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
    void FPSCounter()
    {
        if(m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            m_lastFramerate = (float)m_frameCounter/m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
        m_lastFramerate = Mathf.RoundToInt(m_lastFramerate);
        fpsCounter.text = ( m_lastFramerate.ToString() + " fps");
        if(Input.GetKey(KeyCode.LeftControl))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if(fpsEnabled)
                {
                    fpsCounter.enabled = true;
                    fpsEnabled = false;
                }
                else
                {
                    fpsCounter.enabled = false;
                    fpsEnabled = true;
                }
            }
            else if(Input.GetKeyDown(KeyCode.Y))
            {
                if(highContrast)
                {
                    fpsCounter.color = Color.grey;
                    highContrast = false;              
                }
                else
                {
                    fpsCounter.color = Color.magenta;
                    highContrast = true;
                }
            }
        }
    }
    #endregion
    #region Build Mode
    void EnterBuildMode()
    {
        if(Input.GetKeyDown(KeyCode.B) && !playerController.playerInBuildMode || playerUsedOnScreenButtonBuild == true && !playerController.playerInBuildMode)
        {
            playerController.playerInBuildMode = true;
            playerUsedOnScreenButtonBuild = false;
            buildButton.SetActive(false);
            researchButton.SetActive(false);
            modifyButton.SetActive(false);
            deleteButton.SetActive(false);
            settingsButton.SetActive(false);
            buildMenu.SetActive(true);
        }
    }
    void ExitBuildMode()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && playerController.playerInBuildMode == true)
        {
            playerController.playerInBuildMode = false;
            buildButton.SetActive(true);
            researchButton.SetActive(true);
            modifyButton.SetActive(true);
            deleteButton.SetActive(true);
            settingsButton.SetActive(true);
            buildMenu.SetActive(false);
        }
    }
    public void GoIntoBuildModePhysical()
    {
        playerUsedOnScreenButtonBuild = true;
    }
    public void MultiBuildButton()
    {
        counter++;
        if(counter%2==1 && !playerMultiBuild)
        {
            multiBuild.text = "Multi-Buy: Enabled";
            playerMultiBuild = true;
        }
        else
        {
            multiBuild.text = "Multi-Buy: Disabled";
            playerMultiBuild = false;
        }
    }
    public void OpenCorrespondingInfoMenu(Button button)
    {
        var parent = button.transform.parent.gameObject;
        var infoPanel = getChildGameObject(parent, "InfoPanel");
        if(infoPanel.activeSelf)
        {
            infoPanel.SetActive(false);
        }
        else
        {
            infoPanel.SetActive(true);
        }
    }
    #endregion
    #region Build Mode - Building Information
    
    #endregion
    #region Build Mode - Categories
    void CheckCategoryState()
    {
        if(inIndustryCat)
        {
            defenceMenu.SetActive(false);
            infraMenu.SetActive(false);
            miscMenu.SetActive(false);
            playerCategory = "industry";
        }
        if(inDefenceCat)
        {
            industryMenu.SetActive(false);
            infraMenu.SetActive(false);
            miscMenu.SetActive(false);
            playerCategory = "defence";
        }
        if(inInfraCat)
        {
            industryMenu.SetActive(false);
            defenceMenu.SetActive(false);
            miscMenu.SetActive(false);
            playerCategory = "infra";
        }
        if(inMiscCat)
        {
            industryMenu.SetActive(false);
            defenceMenu.SetActive(false);
            infraMenu.SetActive(false);
            playerCategory = "misc";
        }
    }
    public void IndustryCategoryButton()
    {
        if(!inIndustryCat)
        {
            industryMenu.SetActive(true);
            inIndustryCat = true;
            inMiscCat = false;
            inInfraCat = false;
            inDefenceCat = false;
        }
    }
    public void DefenceCategoryButton()
    {
        if(!inDefenceCat)
        {
            defenceMenu.SetActive(true);
            inDefenceCat = true;
            inMiscCat = false;
            inInfraCat = false;
            inIndustryCat = false;
        }
    }
    public void InfraCategoryButton()
    {
        if(!inInfraCat)
        {
            infraMenu.SetActive(true);
            inInfraCat = true;
            inMiscCat = false;
            inDefenceCat = false;
            inIndustryCat = false;
        }
    }
    public void MiscCategoryButton()
    {
        if(!inMiscCat)
        {
            miscMenu.SetActive(true);
            inMiscCat = true;
            inIndustryCat = false;
            inDefenceCat = false;
            inInfraCat = false;
        }
    }
    #endregion
    #region Settings Menu
    public void EnterSettingsMenu()
    {
        if(Input.GetKeyDown(KeyCode.N) && !playerController.playerInSettings)
        {
            playerController.playerInSettings = true;
            playerUsedOnScreenButtonSettings = false;
            buildButton.SetActive(false);
            researchButton.SetActive(false);
            modifyButton.SetActive(false);
            deleteButton.SetActive(false);
            settingsButton.SetActive(false);
            settingsMenu.SetActive(true);
        }
    }
    public void EnterSettingsPhysical()
    {
        if(!playerUsedOnScreenButtonSettings && !playerController.playerInSettings)
        {
            playerController.playerInSettings = true;
            playerUsedOnScreenButtonSettings = true;
            buildButton.SetActive(false);
            researchButton.SetActive(false);
            modifyButton.SetActive(false);
            deleteButton.SetActive(false);
            settingsButton.SetActive(false);
            settingsMenu.SetActive(true);
        }
    }
    public void LeaveSettingsMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && playerController.playerInSettings)
        {
            playerController.playerInSettings = false;
            playerUsedOnScreenButtonSettings = false;
            buildButton.SetActive(true);
            researchButton.SetActive(true);
            modifyButton.SetActive(true);
            deleteButton.SetActive(true);
            settingsButton.SetActive(true);
            settingsMenu.SetActive(false);
        }
    }
    public void LeaveSettingsPhysical()
    {
        if(playerController.playerInSettings)
        {
            playerController.playerInSettings = false;
            playerUsedOnScreenButtonSettings = false;
            buildButton.SetActive(true);
            researchButton.SetActive(true);
            modifyButton.SetActive(true);
            deleteButton.SetActive(true);
            settingsButton.SetActive(true);
            settingsMenu.SetActive(false);
        }
    }
    #endregion
    #region Settings Menu - Audio
    public void EnterAudioSettings()
    {
        settingsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }
    public void LeaveAudioSettings()
    {
        settingsMenu.SetActive(true);
        audioMenu.SetActive(false);
    }
    #endregion
    #region Settings Menu - Graphics
    public void EnterGraphicsSettings()
    {
        settingsMenu.SetActive(false);
        graphicsMenu.SetActive(true);
    }
    public void LeaveGraphicsSettings()
    {
        settingsMenu.SetActive(true);
        graphicsMenu.SetActive(false);
    }
    #endregion
    #region Notification
    public void CreateNotification(string notificationText, string notificationColour)
    {

    }
    #endregion
    #region Top Area Animations
    public void CreateTextObject(string name, GameObject parent, Color color, Vector3 goal, string useCase, string text, TMPro.TMP_FontAsset font)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent.transform);
        TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
        TextAnimation textAnim = textObject.AddComponent<TextAnimation>();
        tmp.text = text;
        tmp.color = color;
        tmp.font = font;
        tmp.fontSize = 30f;
        tmp.alignment = TextAlignmentOptions.Center;
        textAnim.textObjectAlpha = tmp.color.a;
        textAnim.parentObject = textObject;
        textAnim.parentOfText = parent;
        textAnim.textObject = tmp;
        textAnim.goalPosition = goal;
        textAnim.textValue = text;
        textAnim.font = font;
        textObject.transform.localPosition = Vector3.zero;
        RectTransform canvasRect = textObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1000, 1000);
    }
    public void CreateTextObjectNonAnimated(string name, GameObject parent, Color color, string text, TMPro.TMP_FontAsset font)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent.transform);
        TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
        TextNonAnimated textAnim = textObject.AddComponent<TextNonAnimated>();
        tmp.text = text;
        tmp.color = color;
        tmp.font = font;
        tmp.fontSize = 30f;
        tmp.alignment = TextAlignmentOptions.Center;
        textAnim.parentObject = textObject;
        textAnim.parentOfText = parent;
        textAnim.textObject = tmp;
        textAnim.textValue = text;
        textAnim.font = font;
        textObject.transform.localPosition = Vector3.zero;
        RectTransform canvasRect = textObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1000, 1000);
    }
    #endregion
    #region Configuration / Upgrade UI Functions
    public void CreateButtonFrame(GameObject parentTile, GameObject parentBuilding)
    {
        
    }
    public void CreateUpgradeButton(GameObject parentTile, GameObject parentBuilding, GameObject buttonFrame)
    {
        #region 

        #endregion
    }
    public void CreateConfigureButton(GameObject parentTile, GameObject parentBuilding, GameObject buttonFrame)
    {

    }
    #endregion
}