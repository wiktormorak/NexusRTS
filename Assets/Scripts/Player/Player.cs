using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using FishNet.Managing;

public class Player : MonoBehaviour
{
    public GameObject player;
    public NetworkConnection clientID;
    public GameObject Canvas;
    public UIController uiController;
    public int movementSpeed;
    public Camera playerCamera;
    public Vector3 playerXPOS;
    public Vector3 playerYPOS;
    public Vector3 playerZPOS;
    public GameObject playerOnHoverTile;
    public GameObject playerOnHoverBuilding;
    #region Sync Variables
    [SyncVar] private float _playerCoreHealth;
    public float PlayerCoreHealth {  
        get { return _playerCoreHealth; }
        set { _playerCoreHealth = value; }
    }
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
    #endregion
    #region Integers
    public float playerMovementSpeed;
    #endregion
    #region Booleans
    public bool playerInHeatMap;
    public bool playerInBuildMode;
    public bool playerInConfigMode;
    public bool playerInMultiBuild;
    #endregion
    #region Data Lists
        #region Buildings
        public List<GameObject> houses;
        public List<GameObject> playerVehicleFactories;
        public List<GameObject> playerBaseCamps;
        public List<GameObject> playerAirBases;
        public List<GameObject> playerHarbours;
        public List<GameObject> playerOwnedBuildings;
        public List<Renderer> playerOwnedBuildingsRenderers;
        #endregion
        #region ScriptableObjects
        public List<Building> industryList;
        public List<Building> defenceList;
        public List<Building> infraList;
        public List<Building> miscList;
        #endregion
    #endregion
    #region Object Selection
    public string build_currentlySelected;
    public string build_currentCategory;
    #endregion
    #region ScriptableObjects
    public IndustryList industrySO;
    public DefenceList defenceSO;
    public InfraList infraSO;
    public MiscList miscSO;
    #endregion
    #region Object Placing Vars
    public Dictionary<string, ScriptableObject> categoryMapping;
    public bool build_notInd;
    public bool build_notDef;
    public bool build_notInfra;
    public bool build_notMisc;
    public string build_idName;
    public string build_displayName;
    public string configCategoryTest;
    public GameObject build_toPlace;
    public GameObject build_toReturnBuilding;
    public Vector3 build_prefabOffset;
    public Vector3 build_prefabScale;
    public Vector3 build_prefabRotation;
    public bool build_doesObjectUsePrefabList;
    public List<GameObject> build_objectPrefabList;
    public int build_randomPrefabInt;
    public float build_buildTime;
    public float build_cachedBuildTime;
    public Sprite build_progressBar0Prefab;
    public Sprite build_progressBar25Prefab;
    public Sprite build_progressBar50Prefab;
    public Sprite build_progressBar75Prefab;
    public Sprite build_progressBar100Prefab;
    public float build_progressBarPrefabX;
    public float build_progressBarPrefabY;
    public float build_progressBarPrefabZ;
    public float build_progressBarPrefabScale;
    public bool build_isBuilt;
    public GameObject build_cachedBuilding;
    public GameObject build_cachedTile;
    public Image build_cachedProgressBar;
    #endregion
    #region Text Parent Objects, Colors and Fonts
    public GameObject MoneyAnimate;
    public GameObject ResearchAnimate;
    public GameObject CivilianAnimate;
    public GameObject BottomAnimate;
    public Color moneyColor;
    public Color researchColor;
    public Color civilianColor;
    public Color positiveColor;
    public Color negativeColor;
    public Color normalColor;
    public TMPro.TMP_FontAsset UrbanistMedium;
    public TMPro.TMP_FontAsset UrbanistBold;
    #endregion
    #region Unity Methods
    void Start()
    {
        Invoke("ExternalVars", 1);
        Invoke("SetColours", 1);
        Invoke("InitDictionary", 1);
        Invoke("FindAnimateGameObjects", 1);
        Invoke("SetPlayer", 3);
        Invoke("PlaceHouseStartHandler", 4);
    }
    void Update()
    {
        
    }
    #endregion
    #region Base Methods
    void SetPlayer()
    {
        player = this.gameObject;
        playerCamera = player.GetComponent<Camera>();
        player.transform.position = new Vector3(playerXPOS.x, playerYPOS.y, playerZPOS.z); // -100, 15, -350
        player.transform.rotation = Quaternion.Euler(55,0,0);
    }
    #endregion
    // [ServerRpc] // Clients call this, Server executes it
    // public void TakeDamage(int amount)
    // {
    //     if (!InstanceFinder.ServerManager.IsServer) return; // Extra security check
    //     _playerCoreHealth -= amount;
    //     //NotifyCoreHealthChange(coreHealth);
    // }
}