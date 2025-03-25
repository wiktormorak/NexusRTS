using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class PlayerController : MonoBehaviour
{
    #region Base Vars
    private GameObject player;
    private Camera playerCamera;
    public Vector3 playerXPOS;
    public Vector3 playerYPOS;
    public Vector3 playerZPOS;
    public GameObject playerSpawnTile;
    private GameObject Canvas;
    private Canvas Canvas2;
    public GameObject tileManager;
    public float movementSpeed;
    public bool playerInHeatMap;
    public bool playerInBuildMode;
    public bool playerInConfigMode;
    public bool playerMultiBuild;
    public bool playerInSettings;
    public GameObject playerOnHoverTile;
    public GameObject playerOnHoverBuilding;
    //public GameObject housePrefab;
    public List<GameObject> houses;
    private TileController tileController;
    public List<GameObject> playerVehicleFactories;
    public List<GameObject> playerBaseCamps;
    public List<GameObject> playerAirBases;
    public List<GameObject> playerHarbours;
    #endregion
    #region OnPlayerInit
    private GameObject corePrefab;
    private List<GameObject> tiles;
    #endregion
    #region Tile Data
    private List<Renderer> allTileRenderers;
    private List<Renderer> tier1Renderers;
    private List<Renderer> tier2Renderers;
    private List<Renderer> tier3Renderers;
    private List<Renderer> tier4Renderers;
    private List<Renderer> tier5Renderers;
    public List<Renderer> AllTileRenderers { get => allTileRenderers; private set => allTileRenderers = value; }
    public List<Renderer> Tier1Renderers { get => tier1Renderers; private set => tier1Renderers = value; }
    public List<Renderer> Tier2Renderers { get => tier2Renderers; private set => tier2Renderers = value; }
    public List<Renderer> Tier3Renderers { get => tier3Renderers; private set => tier3Renderers = value; }
    public List<Renderer> Tier4Renderers { get => tier4Renderers; private set => tier4Renderers = value; }
    public List<Renderer> Tier5Renderers { get => tier5Renderers; private set => tier5Renderers = value; }
    #endregion  
    #region Heat Map Mats
    Material grass;
    Material tier1Mat;
    Material tier2Mat;
    Material tier3Mat;
    Material tier4Mat;
    Material tier5Mat;
    Material forestMat;
    #endregion
    #region Coroutine Vars
    public List<GameObject> goodTiles;
    #endregion
    #region Ore Related Vars
    public List<GameObject> oreGenPermittedTiles;
    #endregion
    #region Object Placing Vars
    private Dictionary<string, ScriptableObject> categoryMapping;
    public string build_currentlySelected;
    private string build_currentCategory;
    private bool build_notInd;
    private bool build_notDef;
    private bool build_notInfra;
    private bool build_notMisc;
    private string build_idName;
    private string build_displayName;
    private string configCategoryTest;
    private GameObject build_toPlace;
    private GameObject build_toReturnBuilding;
    public List<GameObject> playerOwnedBuildings;
    public List<Renderer> playerOwnedBuildingsRenderers;
    private Vector3 build_prefabOffset;
    private Vector3 build_prefabScale;
    private Vector3 build_prefabRotation;
    private bool build_doesObjectUsePrefabList;
    private List<GameObject> build_objectPrefabList;
    private int build_randomPrefabInt;
    #endregion
    #region Progress Bar
    private float build_cachedBuildTime;
    private float build_buildTime;
    private Sprite build_progressBar0Prefab;
    private Sprite build_progressBar25Prefab;
    private Sprite build_progressBar50Prefab;
    private Sprite build_progressBar75Prefab;
    private Sprite build_progressBar100Prefab;
    private float build_progressBarPrefabX;
    private float build_progressBarPrefabY;
    private float build_progressBarPrefabZ;
    private float build_progressBarPrefabScale;
    private bool build_isBuilt;
    private GameObject build_cachedBuilding;
    private GameObject build_cachedTile;
    private Image build_cachedProgressBar;
    #endregion
    #region Money Vars
    public float money;
    public float moneyPerSec;
    public float build_buildingPrice;
    #endregion
    #region Research Vars
    public float researchPoints;
    public float researchPointsPerSec;
    #endregion
    #region Civilian Vars
    public int civilianAmount;
    public int houseAmount;
    #endregion
    #region Text Parent Objects, Colors and Fonts
    private GameObject MoneyAnimate;
    private GameObject ResearchAnimate;
    private GameObject CivilianAnimate;
    private GameObject BottomAnimate;
    private Color moneyColor;
    private Color researchColor;
    private Color civilianColor;
    private Color positiveColor;
    private Color negativeColor;
    private Color normalColor;
    private TMPro.TMP_FontAsset UrbanistMedium;
    private TMPro.TMP_FontAsset UrbanistBold;
    #endregion
    #region ScriptableObjects
    public IndustryList industrySO;
    public DefenceList defenceSO;
    public InfraList infraSO;
    public MiscList miscSO;
    #endregion
    #region Lists
    public List<Building> industryList;
    public List<Building> defenceList;
    public List<Building> infraList;
    public List<Building> miscList;
    #endregion
    #region Config Mode Vars
    private GameObject configModePrefab;
    private bool requiresExtraDetailconfig;
    private List<ExtraConfigData> buildingExtrasListconfig;
    private float configPrefabXtemp;
    private float configPrefabYtemp;
    private float configPrefabZtemp;
    private float configPrefabScaletemp;
    private string configBuildingName;
    private int configBuildingID;
    private bool configRequiresSecondButton;
    private string configBuildingExtraText;
    private bool configRequiresExtraDetail;
    private string configFirstButtonUse;

    public string vspawn_selectedVehicle;
    #endregion
    #region Unity Funcs
    void Start()
    {
        Invoke("ExternalVars", 1);
        Invoke("GetMaterials", 1);
        Invoke("SetColours", 1);
        Invoke("InitDictionary", 1);
        Invoke("FindAnimateGameObjects", 1);
        Invoke("SetPlayer", 6);
        Invoke("PlaceHouseStartHandler", 8);
    }
    void Update()
    {
        if(!string.IsNullOrEmpty(build_currentlySelected))
        {
            UpdateCurrentCategory();
        }
        Movement();
        SpeedUpMovement();
        GetHoverInformation();
        ZoomIn();
        ZoomOut();
        PlaceSelected();
        //EnterConfigMode();
        //DisableConfigMode();
        //ConfigureBuilding();
    }
    void OnPlayerInit()
    {
        SetPlayer();
    }
    void CreatePlayerSpawn()
    {
        int random = Random.Range(0, tiles.Count);
        playerSpawnTile = tiles[random];
        if((playerSpawnTile.CompareTag("tier2Tile") || (playerSpawnTile.CompareTag("tier3Tile")
        || (playerSpawnTile.CompareTag("tier3Tile") || (playerSpawnTile.CompareTag("tier4Tile") || (playerSpawnTile.CompareTag("tier5Tile") 
        || (playerSpawnTile.CompareTag("forestTile"))))))))
        {
            Debug.Log("Re-running AssignPlayerSpawn() due to spawnTile being forestTile.");
            CreatePlayerSpawn();
            return;
        }
        //playerSpawnTile.name = ("SPAWN");
        var core = Instantiate(corePrefab);
        core.tag = ("building");
        var pos = playerSpawnTile.transform.position;
        playerSpawnTile.tag = "spawnTile";
        core.transform.position = new Vector3(pos.x, pos.y + 0.25f, pos.z);
        core.transform.SetParent(playerSpawnTile.transform);
        playerXPOS = (transform.TransformPoint(new Vector3(playerSpawnTile.transform.position.x,0f,0f))); //- new Vector3(20f,0,0));
        playerYPOS = (new Vector3(0f,15f,0f));
        playerZPOS = (transform.TransformPoint(new Vector3(0f,0f,playerSpawnTile.transform.position.z) - new Vector3(0f,0f,7.5f)));
        PlaceHouseStartHandler();
    }
    void SetPlayer()
    {
        player = this.gameObject;
        playerCamera = player.GetComponent<Camera>();
        player.transform.position = new Vector3(playerXPOS.x, playerYPOS.y, playerZPOS.z); // -100, 15, -350
        player.transform.rotation = Quaternion.Euler(55,0,0);
    }
    void SetColours()
    {
        moneyColor = new Color(0.388f, 0.709f, 0.203f, 1f);
        researchColor = new Color(0f, 0.478f, 0.929f, 1f);
        civilianColor = new Color(0.929f, 0.603f, 0f, 1f);
        positiveColor = new Color(0.376f, 0.643f, 0.223f, 1f);
        negativeColor = new Color(1f, 0f, 0f, 1f);
        normalColor = new Color(0.941f, 0.921f, 0.898f, 1f);
    }
    void FindAnimateGameObjects()
    {
        MoneyAnimate = GameObject.FindWithTag("MoneyAnimate");
        ResearchAnimate = GameObject.FindWithTag("ResearchAnimate");
        CivilianAnimate = GameObject.FindWithTag("CivilianAnimate");
        BottomAnimate = GameObject.FindWithTag("BottomAnimate");
    }
    void ExternalVars()
    {
        corePrefab = Resources.Load("core", typeof(GameObject)) as GameObject;
        Canvas = GameObject.FindWithTag("UIController");
        tileManager = GameObject.FindWithTag("tileManager");
        tileController = tileManager.GetComponent<TileController>();
        UrbanistMedium = Canvas.GetComponent<UIController>().UrbanistMedium;
        UrbanistBold = Canvas.GetComponent<UIController>().UrbanistBold;
        tiles = tileController.tiles;
    }
    void InitDictionary()
    {
        categoryMapping = new Dictionary<string, ScriptableObject>
        {
            { "industry", industrySO },
            { "defence", defenceSO },
            { "infra", infraSO },
            { "misc", miscSO }
        };
    }
    void UpdateCurrentCategory()
    {
        build_currentCategory = Canvas.GetComponent<UIController>().playerCategory;
    }
    void GetMaterials()
    {
        #region Heat Map
        grass = Resources.Load("Grass", typeof(Material)) as Material;
        tier1Mat = Resources.Load("Tier1", typeof(Material)) as Material;
        tier2Mat = Resources.Load("Tier2", typeof(Material)) as Material;
        tier3Mat = Resources.Load("Tier3", typeof(Material)) as Material;
        tier4Mat = Resources.Load("Tier4", typeof(Material)) as Material;
        tier5Mat = Resources.Load("Tier5", typeof(Material)) as Material;
        forestMat = Resources.Load("forestTile", typeof(Material)) as Material;
        #endregion
    }
    #endregion
    #region Utility Functions
    public void SetParentAndPosition(GameObject building, GameObject parent, Vector3 position)
    {
        building.transform.SetParent(parent.transform);
        building.transform.position = position;
    }
    public void SetNameAndTag(GameObject building, string name, string tag)
    {
        building.name = name;
        building.tag = tag;
    }
    public void AddCivilians(int civiliansToAdd)
    {
        civilianAmount = civilianAmount + civiliansToAdd;
    }
    public void AddHouses(int housesToAdd)
    {
        houseAmount = houseAmount + housesToAdd;
    }
    public void AddToOwnedBuildings(GameObject building)
    {
        playerOwnedBuildings.Add(building);
        playerOwnedBuildingsRenderers.Add(building.GetComponent<Renderer>());
    }
    #endregion
    #region Movement Related
    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        if (movement.magnitude > 1f)
        {
            movement = movement.normalized;
        }
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
    }
    void SpeedUpMovement()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(movementSpeed >= 30)
            {
                return;
            }
            movementSpeed = movementSpeed * 2;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = 15;
        }
    }
    void ZoomIn()
    {
        if(Input.GetKey(KeyCode.E) && player.GetComponent<Camera>().fieldOfView >= 11)
        {
            player.GetComponent<Camera>().fieldOfView--;
        }
    }
    void ZoomOut()
    {
        if(Input.GetKey(KeyCode.Q) && player.GetComponent<Camera>().fieldOfView <= 62)
        {
            player.GetComponent<Camera>().fieldOfView++;
        }
    }
    #endregion
    #region Object Placing
    void CreateProgressBar(GameObject player, float buildTime, GameObject parent, GameObject building)
    {
        building.AddComponent<ProgressBarHandler>();
        building.GetComponent<ProgressBarHandler>().player = player;
        building.GetComponent<ProgressBarHandler>().parent = parent;
        building.GetComponent<ProgressBarHandler>().building = building;
        building.GetComponent<ProgressBarHandler>().tempBuildTime = buildTime;
    }
    public void GetSelectedBuilding(Button button)
    {
        string parentName = button.transform.name;
        build_currentlySelected = parentName;
    }
    public GameObject FindBuildingByName()
    {
        #region list
        var temporary = new List<List<Building>>()
        {
            industryList,
            defenceList,
            infraList,
            miscList
        };
        #endregion
        #region Store Data
        void Store(Building listBuilding)
        {
            build_toPlace = listBuilding.prefab;
            build_idName = listBuilding.idName;
            build_displayName = listBuilding.displayName;
            build_buildingPrice = listBuilding.buildingCost;
            build_prefabOffset = listBuilding.prefabOffset;
            build_prefabScale = listBuilding.prefabScale;
            build_prefabRotation = listBuilding.prefabRotation;
            build_doesObjectUsePrefabList = listBuilding.usePrefabLists;
            if(build_doesObjectUsePrefabList)
            {
                build_objectPrefabList = listBuilding.prefabList;
                build_toPlace = build_objectPrefabList[Random.Range(0, build_objectPrefabList.Count)];
            }
            build_buildTime = listBuilding.buildTime;
            build_progressBarPrefabX = listBuilding.progressBarPrefabX;
            build_progressBarPrefabY = listBuilding.progressBarPrefabY;
            build_progressBarPrefabZ = listBuilding.progressBarPrefabZ;
            build_progressBarPrefabScale = listBuilding.progressBarPrefabScale;
        }
        #endregion
        #region Find Building By Name
        if (categoryMapping.TryGetValue(build_currentCategory, out var categorySO))
        {
            #region industry list
            if (categorySO is IndustryList industrySO)
            {
                foreach (var building in temporary[0])
                {
                    if (building != null && building.idName == build_currentlySelected)
                    {
                        Store(building);
                    }
                }
                return build_toPlace;
            }
            #endregion
            #region defense list
            else if (categorySO is DefenceList defenceSO)
            {
                foreach (var building in temporary[1])
                {
                    if (building != null && building.idName == build_currentlySelected)
                    {
                        Store(building);
                    }
                }
                return build_toPlace;
            }
            #endregion
            #region infra list
            else if (categorySO is InfraList infraSO)
            {
                foreach (var building in temporary[2])
                {
                    if (building != null && building.idName == build_currentlySelected)
                    {
                        Store(building);
                    }
                }
                return build_toPlace;
            }
            #endregion
            #region misc list
            else if (categorySO is MiscList miscSO)
            {
                foreach (var building in temporary[3])
                {
                    if (building != null && building.idName == build_currentlySelected)
                    {
                        Store(building);
                    }
                }
                return build_toPlace;
            }
            #endregion
        }
        return build_toPlace;
        #endregion
    }
    // #region Progress Bar 
    // void Store(Building listBuilding)
    // {
    //     configFirstButtonUse = listBuilding.firstButtonUse.ToString();
    //     configBuildingExtraText = listBuilding.buildingExtraText;
    //     configRequiresExtraDetail = listBuilding.requiresExtraDetail;
    //     configBuildingName = listBuilding.displayName;
    //     configBuildingID = listBuilding.buildingID;
    //     configModePrefab = listBuilding.configPrefab;
    //     configRequiresSecondButton = listBuilding.requiresSecondButton;
    //     requiresExtraDetailconfig = listBuilding.requiresExtraDetail;
    //     buildingExtrasListconfig = listBuilding.buildingExtrasList;
    //     configPrefabXtemp = listBuilding.configPrefabX;
    //     configPrefabYtemp = listBuilding.configPrefabY;
    //     configPrefabZtemp = listBuilding.configPrefabZ;
    //     configPrefabScaletemp = listBuilding.configPrefabScale;
    // }
    // void IndustryCheck(GameObject buildingToFind)
    // {
    //     #region List
    //     var temporary = new List<List<Building>>()
    //     {
    //         industryList,
    //         defenceList,
    //         infraList,
    //         miscList
    //     };
    //     #endregion
    //     #region Check
    //     build_notInd = true;
    //     if (categoryMapping.TryGetValue(configCategoryTest, out var categorySO))
    //     {
    //         if (categorySO is IndustryList industrySO)
    //         {
    //             foreach (var building in temporary[0])
    //             {
    //                 if (building != null && building.idName == buildingToFind.name)
    //                 {
    //                     Store(building);
    //                     build_notInd = false;
    //                     return;
    //                 }
    //             }
    //         }
    //     }
    //     if(build_notInd)
    //     {
    //         configCategoryTest = "defense";
    //         DefenseCheck(buildingToFind);
    //     }
    //     #endregion
    // }
    // void DefenseCheck(GameObject buildingToFind)
    // {
    //     #region List
    //     var temporary = new List<List<Building>>()
    //     {
    //         industryList,
    //         defenceList,
    //         infraList,
    //         miscList
    //     };
    //     #endregion
    //     #region Check
    //     build_notDef = true;
    //     if (categoryMapping.TryGetValue(configCategoryTest, out var categorySO))
    //     {
    //         if (categorySO is DefenceList defenceSO)
    //         {
    //             foreach (var building in temporary[1])
    //             {
    //                 if (building != null && building.idName == buildingToFind.name)
    //                 {
    //                     Store(building);
    //                     build_notDef = false;
    //                     return;
    //                 }
    //             }
    //         }
    //     }
    //     if (build_notDef)
    //     {
    //         configCategoryTest = "infra";
    //         InfraCheck(buildingToFind);
    //     }
    //     #endregion
    // }
    // void InfraCheck(GameObject buildingToFind)
    // {
    //     #region List
    //     var temporary = new List<List<Building>>()
    //     {
    //         industryList,
    //         defenceList,
    //         infraList,
    //         miscList
    //     };
    //     #endregion
    //     #region Check
    //     build_notInfra = true;
    //     if (categoryMapping.TryGetValue(configCategoryTest, out var categorySO))
    //     {
    //         if (categorySO is InfraList infraSO)
    //         {
    //             foreach (var building in temporary[2])
    //             {
    //                 if (building != null && building.idName == buildingToFind.name)
    //                 {
    //                     Store(building);
    //                     build_notInfra = false;
    //                     return;
    //                 }
    //             }
    //         }
    //     }
    //     if (build_notInfra)
    //     {
    //         configCategoryTest = "misc";
    //         MiscCheck(buildingToFind);
    //     }
    //     #endregion
    // }
    // void MiscCheck(GameObject buildingToFind)
    // {
    //     #region List
    //     var temporary = new List<List<Building>>()
    //     {
    //         industryList,
    //         defenceList,
    //         infraList,
    //         miscList
    //     };
    //     #endregion
    //     #region Check
    //     build_notMisc = true;
    //     if (categoryMapping.TryGetValue(configCategoryTest, out var categorySO))
    //     {
    //         if (categorySO is MiscList miscSO)
    //         {
    //             foreach (var building in temporary[3])
    //             {
    //                 if (building != null && building.idName == buildingToFind.name)
    //                 {
    //                     Store(building);
    //                     build_notMisc = false;
    //                     return;
    //                 }
    //             }
    //         }
    //     }
    //     #endregion
    // }
    // #endregion
    void PlaceSelected()
    {
        if(Input.GetMouseButtonDown(0) && !string.IsNullOrEmpty(build_currentlySelected) && playerInBuildMode && playerOnHoverTile.transform.childCount == 0)
        {   
            var selectedPrefab = FindBuildingByName();
            GameObject placedObject = transform.gameObject;
            if(build_buildingPrice <= money)
            {
                #region Instaniate
                placedObject = Instantiate(selectedPrefab);
                #endregion
                #region Set PlacedObject Tags
                placedObject.transform.SetParent(playerOnHoverTile.transform);
                placedObject.name = build_idName;
                placedObject.tag = "building";
                placedObject.transform.position = new Vector3(playerOnHoverTile.transform.position.x + build_prefabOffset.x, playerOnHoverTile.transform.position.y + build_prefabOffset.y, playerOnHoverTile.transform.position.z + build_prefabOffset.z);
                placedObject.transform.localScale = new Vector3(build_prefabScale.x,build_prefabScale.y,build_prefabScale.z);
                placedObject.transform.localRotation = Quaternion.Euler(build_prefabRotation.x, build_prefabRotation.y, build_prefabRotation.z);
                #endregion
                #region Progress Bar
                build_cachedTile = playerOnHoverTile;
                build_cachedBuilding = placedObject;
                CreateProgressBar(player, build_buildTime, build_cachedTile, build_cachedBuilding);
                #endregion
                #region Remove Money
                money -= build_buildingPrice;
                Canvas.GetComponent<UIController>().CreateTextObject("Text", MoneyAnimate, moneyColor, new Vector3(0f,40f,0f), "negative", ("-" + build_buildingPrice.ToString()), UrbanistBold);
                #endregion
                #region Multi-Build
                if(!playerMultiBuild)
                {
                    build_currentlySelected = ("");
                    build_buildingPrice = 0f;
                }
                #endregion
            }
            else if(build_buildingPrice > money)
            {
                Canvas.GetComponent<UIController>().CreateTextObject("Text", BottomAnimate, negativeColor, new Vector3(0f,60f,0f), "negative", ("Not enough money to build a " + build_displayName), UrbanistMedium);
                build_currentlySelected = ("");
                build_buildingPrice = 0f;
            }
        }
    }
    public void PlaceHouseStartHandler()
    {
        if(tileController != null)
        {
            #region Run getsurroundingtiles
            var potentialTiles = tileController.GetSurroundingTiles(playerSpawnTile, 3);
            #endregion
            #region Store Good Tiles && Set areOresGenerated to True
            foreach(List<GameObject> layer in potentialTiles)
            {
                foreach(GameObject tile in layer)
                {
                    if(tile.CompareTag("tier1Tile") || tile.CompareTag("tier2Tile") || tile.CompareTag("tier3Tile") || tile.CompareTag("tier4Tile") || tile.CompareTag("tier5Tile"))
                    {
                        oreGenPermittedTiles.Add(tile);
                        foreach(GameObject tile2 in oreGenPermittedTiles)
                        {
                            tile2.GetComponent<TileOreData>().oreGenerate = true;
                        }
                    }
                    if(tile.CompareTag("tier1Tile"))
                    {
                        goodTiles.Add(tile);
                    }
                }
            }
            #endregion
            #region RNG
            GameObject playerHouseTile1 = goodTiles[Random.Range(0, goodTiles.Count)];
            GameObject playerHouseTile2 = goodTiles[Random.Range(0, goodTiles.Count)];
            if(playerHouseTile2 == playerHouseTile1)
            {
                playerHouseTile2 = goodTiles[Random.Range(0, goodTiles.Count)];
            }
            #endregion
            int housesToBuild = 2;
            for(int i = 0; i < housesToBuild; i++)
            {
                var playerHouseTile = this.transform.gameObject;
                if(i == 1)
                {
                    playerHouseTile = playerHouseTile1;
                }
                else
                {
                    playerHouseTile = playerHouseTile2;
                }
                int randomHouseNum = Random.Range(0, houses.Count);
                var randomHouse = houses[randomHouseNum];
                var placedHouse = Instantiate(randomHouse);
                var pos1 = playerHouseTile.transform.position;
                SetNameAndTag(placedHouse, "house", "building");
                SetParentAndPosition(placedHouse, playerHouseTile, new Vector3(pos1.x, pos1.y + 0.1f, pos1.z + 0.1f));
                AddHouses(1);
                AddCivilians(2);
                AddToOwnedBuildings(placedHouse);
            }
            Canvas.GetComponent<UIController>().CreateTextObject("Text", CivilianAnimate, civilianColor, new Vector3(0f,40f,0f), "none", ("+4"), UrbanistBold);
        }
    }
    void GetHoverInformation()
    {
        if(playerCamera != null)
        {
            //int layerMask = 1 << 7;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                #region Check for Child Count
                if(hit.collider.gameObject.transform.childCount <= 0)
                {
                    if(hit.collider.gameObject.CompareTag("tier1Tile") || (hit.collider.gameObject.CompareTag("tier2Tile") || (hit.collider.gameObject.CompareTag("tier3Tile")
                    || (hit.collider.gameObject.CompareTag("tier3Tile") || (hit.collider.gameObject.CompareTag("tier4Tile") || (hit.collider.gameObject.CompareTag("tier5Tile") 
                    || (hit.collider.gameObject.CompareTag("forestTile"))))))))
                    {
                        playerOnHoverTile = hit.collider.gameObject;
                    }
                }
                else if(hit.collider.gameObject.transform.childCount > 0)
                {
                    var child = hit.collider.gameObject.transform.GetChild(0);
                    if(child.CompareTag("building"))
                    {
                        playerOnHoverBuilding = child.gameObject;
                    }
                }
                #endregion
                #region Simple Check for Building
                if(hit.collider.gameObject.CompareTag("building"))
                {
                    playerOnHoverBuilding = hit.collider.gameObject;
                }
                #endregion
            }
        }
    }
    #endregion
    #region Heat Map
    void EnableHeatMap()
    {
        if(Input.GetKeyDown(KeyCode.H) && !playerInHeatMap && !playerInConfigMode && !playerInBuildMode)
        {
            playerInHeatMap = true;
            foreach (Renderer tier1renderer in tier1Renderers)
            {
                tier1renderer.material = tier1Mat;
            }
            foreach (Renderer tier2renderer in tier2Renderers)
            {
                tier2renderer.material = tier2Mat;
            }
            foreach (Renderer tier3renderer in tier3Renderers)
            {
                tier3renderer.material = tier3Mat;
            }
            foreach (Renderer tier4renderer in tier4Renderers)
            {
                tier4renderer.material = tier4Mat;
            }
            foreach (Renderer tier5renderer in tier5Renderers)
            {
                tier5renderer.material = tier5Mat;
            }
        }
    }
    void DisableHeatMap()
    {
        if(Input.GetKeyDown(KeyCode.H) && playerInHeatMap)
        {
            playerInHeatMap = false;
            foreach (Renderer tier1renderer in tier1Renderers)
            {
                tier1renderer.material = grass;
            }
            foreach (Renderer tier2renderer in tier2Renderers)
            {
                tier2renderer.material = grass;
            }
            foreach (Renderer tier3renderer in tier3Renderers)
            {
                tier3renderer.material = grass;
            }
            foreach (Renderer tier4renderer in tier4Renderers)
            {
                tier4renderer.material = grass;
            }
            foreach (Renderer tier5renderer in tier5Renderers)
            {
                tier5renderer.material = grass;
            }
        }
    }
    #endregion
    #region Config Mode
    void EnterConfigMode()
    {
        if(Input.GetKeyDown(KeyCode.C) && !playerInConfigMode && !playerInBuildMode && !playerInHeatMap)
        {
            //ChangeBoolState(true);
            tileManager.GetComponent<TileController>().EnableConfigMode();
            Canvas.GetComponent<UIController>().CreateTextObjectNonAnimated("ConfigText", BottomAnimate, normalColor, ("Select buildings to configure"), UrbanistBold);
        }
    }
    void DisableConfigMode()
    {
        if(Input.GetKeyDown(KeyCode.C) && playerInConfigMode)
        {
            //ChangeBoolState(false);
            tileManager.GetComponent<TileController>().DisableConfigMode();
        }
    }
    void SelectBuilding()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            List<GameObject> config_buildingList = new List<GameObject>();
            config_buildingList.Add(playerOnHoverBuilding);
            if(config_buildingList[1] != config_buildingList[0])
            {
                config_buildingList.RemoveAt(1);
            }
        }
    }
    // void ChangeBoolState(bool boolean)
    // {
    //     playerInConfigMode = boolean;
    // }
    // void ConfigureBuilding()
    // {
    //     if(playerInConfigMode && playerOnHoverBuilding && playerOnHoverBuilding.transform.childCount <= 0)
    //     {
    //         var buildingToConfig = gameObject;
    //         if(Input.GetMouseButtonDown(0))
    //         {
    //             buildingToConfig = playerOnHoverBuilding;
    //             CreateConfigMenu(buildingToConfig, playerOnHoverTile);
    //         }
    //     }
    // }
    // void CreateConfigMenu(GameObject building, GameObject parentTile)
    // {
    //     configCategoryTest = ("industry");
    //     if (!build_notInd)
    //     {
    //         IndustryCheck(building);
    //     }
    //     var parent = building.transform;
    //     var configPrefab = Instantiate(configModePrefab);
    //     #region UI Main
    //     configPrefab.GetComponent<Canvas>().worldCamera = player.GetComponent<Camera>();
    //     configPrefab.transform.SetParent(parent.transform);
    //     configPrefab.transform.localPosition = new Vector3(configPrefabXtemp, configPrefabYtemp, configPrefabZtemp);
    //     configPrefab.transform.localScale = new Vector3(configPrefabScaletemp,configPrefabScaletemp,configPrefabScaletemp);
    //     #endregion
    //     #region Get Children
    //     var buildingNameText = configPrefab.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>();
    //     var buildingIDText = configPrefab.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TMP_Text>();
    //     var buildingExtraTextParent = configPrefab.transform.GetChild(0).GetChild(2);
    //     var buildingExtraText = configPrefab.transform.GetChild(0).GetChild(2).GetComponent<TMPro.TMP_Text>();
    //     var buildingFirstButton = configPrefab.transform.GetChild(0).GetChild(3);
    //     var buildingFirstButtonText = configPrefab.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TMPro.TMP_Text>();
    //     var buildingSecondButton = configPrefab.transform.GetChild(0).GetChild(4);
    //     var buildingSecondButtonText = configPrefab.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<TMPro.TMP_Text>();
    //     #endregion
    //     #region Set Text
    //     buildingNameText.text = (configBuildingName.ToString());
    //     buildingIDText.text = ("(" + configBuildingID.ToString() + ")");
    //     buildingExtraText.text = (configBuildingExtraText.ToString());
    //     #endregion
    //     #region Disable Second Button & Extra Detail Text
    //     if(!configRequiresSecondButton)
    //     {
    //         buildingSecondButton.gameObject.SetActive(false);
    //     }
    //     if(!configRequiresExtraDetail)
    //     {
    //         buildingExtraTextParent.gameObject.SetActive(false);
    //     }
    //     #endregion
    //     #region Set First Button Text
    //     buildingFirstButtonText.text = (configFirstButtonUse.ToUpper());
    //     #endregion
    // }
    #endregion
}