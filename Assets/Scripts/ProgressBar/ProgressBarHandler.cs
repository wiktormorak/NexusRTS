using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class ProgressBarHandler : MonoBehaviour
{
    #region UI
    public GameObject UIControllerObject;
    public UIController uiController;
    #endregion
    #region Text
    public TMPro.TMP_FontAsset UrbanistMedium;
    public TMPro.TMP_FontAsset UrbanistBold;
    #endregion
    #region Text Parent Objects
    private GameObject MoneyAnimate;
    private GameObject ResearchAnimate;
    private GameObject CivilianAnimate;
    private GameObject BottomAnimate;
    private Color moneyColor;
    private Color researchColor;
    private Color civilianColor;
    private Color positiveColor;
    private Color negativeColor;
    #endregion
    #region Player
    public GameObject player;
    private PlayerController playerController;
    private List<GameObject> ext_playerOwnedBuildings;
    private List<Renderer> ext_playerOwnedBuildingsRenderers;
    private int ext_civilianAmount;
    private int ext_houseAmount;
    private float ext_tempBuildingPrice;
    private float ext_playerMoney;
    private float ext_buildingPrice;
    private bool ext_multiBuild;
    private string ext_currentlySelected;
    #endregion
    #region Progress Bar
    private bool enableProgressBar;
    private bool completedSetup;
    public GameObject parent;
    public GameObject building;
    private float cachedTempBuildTime;
    public float tempBuildTime;
    public Sprite tempProgressBar0Prefab;
    public Sprite tempProgressBar25Prefab;
    public Sprite tempProgressBar50Prefab;
    public Sprite tempProgressBar75Prefab;
    public Sprite tempProgressBar100Prefab;
    private float tempProgressBarPrefabX;
    private float tempProgressBarPrefabY;
    private float tempProgressBarPrefabZ;
    private float tempProgressBarPrefabScale;
    private bool isBuilt;
    private Image cachedProgressBar;
    #endregion
    #region moved vars
    private Canvas canvasObject;
    private GameObject progressBarParent;
    private Image progressBar;
    #endregion
    #region Unity and External Methods
    void Start()
    {
        Invoke("StoreText", 1);
        Invoke("SetColours", 1);
        Invoke("FindAnimateGameObjects", 1);
        cachedTempBuildTime = tempBuildTime;
        enableProgressBar = true;
        tempProgressBar0Prefab = Resources.Load("ProgressBar0", typeof(Sprite)) as Sprite;
        tempProgressBar25Prefab = Resources.Load("ProgressBar25", typeof(Sprite)) as Sprite;
        tempProgressBar50Prefab = Resources.Load("ProgressBar50", typeof(Sprite)) as Sprite;
        tempProgressBar75Prefab = Resources.Load("ProgressBar75", typeof(Sprite)) as Sprite;
        tempProgressBar100Prefab = Resources.Load("ProgressBar100", typeof(Sprite)) as Sprite;
    }
    void Update()
    {
        StoreExternalData();
        if(enableProgressBar)
        {
            EnableProgressBar(parent, building);
        }
    }
    void StoreText()
    {
        UrbanistMedium = Resources.Load<TMP_FontAsset>("Fonts/Urbanist-Medium");
        UrbanistBold = Resources.Load<TMP_FontAsset>("Fonts/Urbanist-Bold");
    }
    void SetColours()
    {
        moneyColor = new Color(0.388f, 0.709f, 0.203f, 1f);
        researchColor = new Color(0f, 0.478f, 0.929f, 1f);
        civilianColor = new Color(0.929f, 0.603f, 0f, 1f);
        positiveColor = new Color(0.376f, 0.643f, 0.223f, 1f);
        negativeColor = new Color(1f, 0f, 0f, 1f);
    }
    void FindAnimateGameObjects()
    {
        MoneyAnimate = GameObject.FindWithTag("MoneyAnimate");
        ResearchAnimate = GameObject.FindWithTag("ResearchAnimate");
        CivilianAnimate = GameObject.FindWithTag("CivilianAnimate");
        BottomAnimate = GameObject.FindWithTag("BottomAnimate");
    }
    void StoreExternalData()
    {
        UIControllerObject = GameObject.FindWithTag("UIController");
        uiController = UIControllerObject.GetComponent<UIController>();
        playerController = player.GetComponent<PlayerController>();
        ext_playerOwnedBuildings = playerController.playerOwnedBuildings;
        ext_playerOwnedBuildingsRenderers = playerController.playerOwnedBuildingsRenderers;
        ext_playerMoney = playerController.money;
        ext_multiBuild = playerController.playerMultiBuild;
        ext_civilianAmount = playerController.civilianAmount;
        ext_houseAmount = playerController.houseAmount;
        ext_currentlySelected = playerController.build_currentlySelected;
        ext_tempBuildingPrice = playerController.build_buildingPrice;
    }
    #endregion
    #region Progress Bar Util
    GameObject CreateProgressBarObject()
    {
        var gameObject = new GameObject("ProgressBarParent");
        gameObject.transform.localScale = new Vector3(tempProgressBarPrefabScale,tempProgressBarPrefabScale,tempProgressBarPrefabScale);
        gameObject.transform.localPosition = new Vector3(tempProgressBarPrefabX, tempProgressBarPrefabY, tempProgressBarPrefabZ);
        return gameObject;
    }
    Canvas CreateBuildTimerCanvas(GameObject parent)
    {
        var canvasParent = new GameObject("Temporary");
        canvasParent.transform.SetParent(parent.transform);
        var canvas = canvasParent.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = this.gameObject.GetComponent<Camera>();
        return canvas;
    }
    Image CreateBTBackground(GameObject parentObject, Canvas canvas)
    {
        var panelObject = new GameObject("Background");
        panelObject.gameObject.transform.SetParent(parentObject.transform);
        Image panelImage = panelObject.AddComponent<Image>();
        panelImage.preserveAspect = true;
        return panelImage;
    }
    #endregion
    #region Progress Bar Main
    void EnableProgressBar(GameObject parent, GameObject building)
    {
        StartCoroutine(ProgressBarCoroutine(parent, building));
    }
    void CreateProgressBar(GameObject parent, GameObject building)
    {
        #region Time
        if(!completedSetup)
        {
            #region Canvas
            canvasObject = CreateBuildTimerCanvas(parent);
            canvasObject.transform.SetParent(parent.transform);
            #endregion
            #region recttransform
            RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
            canvasRect.localRotation = Quaternion.Euler(55f,0f,0f);
            canvasRect.localPosition = new Vector3(0f,0f,0f);
            canvasRect.sizeDelta = new Vector2(1920, 1080);
            canvasRect.localScale = new Vector3(1f,1f,1f);
            #endregion
            #region Background and Progress Bar
            progressBarParent = CreateProgressBarObject();
            progressBarParent.transform.SetParent(canvasObject.transform);
            progressBarParent.transform.position = new Vector3(building.transform.position.x, building.transform.position.y, building.transform.position.z);
            progressBar = CreateBTBackground(progressBarParent, canvasObject);
            progressBarParent.transform.localPosition = new Vector3(0f,0.011f,0.006f);
            Quaternion rotation = Quaternion.Euler(-55f,180f,180f);
            progressBarParent.transform.rotation = rotation;
            progressBar.transform.localScale = new Vector3(1f,1f,1f);
            progressBarParent.transform.localScale = new Vector3(0.0002f,0.0002f,0.0002f);
            progressBar.transform.localPosition = new Vector3(0f,0f,0f);
            #endregion
            completedSetup = true;
        }
        #endregion
        #region logic
        var singleStep = cachedTempBuildTime / 4;
        if (tempBuildTime >= 0f)
        {
            progressBar.sprite = tempProgressBar0Prefab;
        }
        if (tempBuildTime <= singleStep * 3)
        {
            progressBar.sprite = tempProgressBar25Prefab;
        }
        if (tempBuildTime <= singleStep * 2) 
        {
            progressBar.sprite = tempProgressBar50Prefab;
        }
        if (tempBuildTime <= singleStep) 
        {
            progressBar.sprite = tempProgressBar75Prefab;
        }
        if (tempBuildTime <= 0) 
        {
            progressBar.sprite = tempProgressBar100Prefab;
        }
        #endregion
    }
    IEnumerator ProgressBarCoroutine(GameObject parent, GameObject building)
    {
        tempBuildTime = tempBuildTime - Time.deltaTime;
        CreateProgressBar(parent, building);
        if(tempBuildTime <= 0)
        {
            SetExternalVarsOnIsBuilt();
            isBuilt = true;
            enableProgressBar = false;
            DeleteSelf();
        }
        yield return null;
    }
    #endregion
    #region Finishing Touches
    void SetExternalVarsOnIsBuilt()
    {
        playerController.playerOwnedBuildings.Add(building);
        playerController.playerOwnedBuildingsRenderers.Add(building.GetComponent<Renderer>());
        #region If House
        if(building.name == "house")
        {
            playerController.houseAmount++;
            playerController.civilianAmount = playerController.civilianAmount + 2;
            uiController.CreateTextObject("Text", CivilianAnimate, civilianColor, new Vector3(0f,40f,0f), "none", ("+2"), UrbanistBold);
        }
        else if(building.name == "vehiclefac")
        {
            playerController.playerVehicleFactories.Add(building);
        }
        else if(building.name == "basecamp")
        {
            playerController.playerBaseCamps.Add(building);
        }
        #endregion
    }
    void DeleteSelf()
    {
        var thisScript = this.parent.GetComponent<ProgressBarHandler>();
        Destroy(thisScript);
        Destroy(canvasObject);
        Destroy(progressBarParent);
        Destroy(progressBar);
    }
    #endregion
}