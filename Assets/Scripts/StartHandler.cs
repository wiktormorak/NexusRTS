using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Transporting;

public class StartHandler : MonoBehaviour
{   
    #region Prefabs
    public GameObject corePrefab;
    public GameObject housePrefab;
    #endregion
    #region Player Vars
    public GameObject player;
    private Vector3 playerXPos;
    private Vector3 playerYPos;
    private Vector3 playerZPos;
    //public List<GameObject> players;
    #endregion
    #region External Vars
    public GameObject tileManager;
    public TileController tileController;
    public List<GameObject> tiles;
    public PlayerController playerController;
    public UIController uiController;
    public GameObject playerSpawnTile;
    #endregion
    #region Text Parent Objects && Colors
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
    #region Text
    public TMPro.TMP_FontAsset UrbanistMedium;
    public TMPro.TMP_FontAsset UrbanistBold;
    #endregion
    #region Unity Funcs
    void Start()
    {
        Invoke("FindAnimateGameObjects", 1);
        //Invoke("GetPlayers", 1);
        Invoke("StoreText", 1);
        Invoke("SetColours", 1);
        Invoke("AddMoneyToPlayers", 2);
        Invoke("AddResearchToPlayers", 2);
        Invoke("AssignPlayerSpawn", 2);
    }
    void Update()
    {
        GetExternalVars();
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
    #endregion
    //void GetPlayers()
    //{
        //players.Add(GameObject.FindWithTag("Player"));
    //}
    void GetExternalVars()
    {
        tileController = tileManager.GetComponent<TileController>();
        playerController = player.GetComponent<PlayerController>();
        uiController = GameObject.FindWithTag("UIController").GetComponent<UIController>();
        playerController.playerSpawnTile = playerSpawnTile;
        playerController.playerXPOS = playerXPos;
        playerController.playerYPOS = playerYPos;
        playerController.playerZPOS = playerZPos;
        tiles = tileController.tiles;
    }
    void AssignPlayerSpawn()
    {
        int random = Random.Range(0, tiles.Count);
        playerSpawnTile = tiles[random];
        if((playerSpawnTile.CompareTag("tier2Tile") || (playerSpawnTile.CompareTag("tier3Tile")
        || (playerSpawnTile.CompareTag("tier3Tile") || (playerSpawnTile.CompareTag("tier4Tile") || (playerSpawnTile.CompareTag("tier5Tile") 
        || (playerSpawnTile.CompareTag("forestTile"))))))))
        {
            Debug.Log("Re-running AssignPlayerSpawn() due to spawnTile being forestTile.");
            AssignPlayerSpawn();
            return;
        }
        //playerSpawnTile.name = ("SPAWN");
        var core = Instantiate(corePrefab);
        core.tag = ("building");
        var pos = playerSpawnTile.transform.position;
        playerSpawnTile.tag = "spawnTile";
        core.transform.position = new Vector3(pos.x, pos.y + 0.25f, pos.z);
        core.transform.SetParent(playerSpawnTile.transform);
        playerXPos = (transform.TransformPoint(new Vector3(playerSpawnTile.transform.position.x,0f,0f))); //- new Vector3(20f,0,0));
        playerYPos = (new Vector3(0f,15f,0f));
        playerZPos = (transform.TransformPoint(new Vector3(0f,0f,playerSpawnTile.transform.position.z) - new Vector3(0f,0f,7.5f)));
        playerController.PlaceHouseStartHandler();
    }
    void AddMoneyToPlayers()
    {
        playerController.money += 9999999f;
        uiController.CreateTextObject("Text", MoneyAnimate, moneyColor, new Vector3(0f,40f,0f), "none", ("+ 9,999,999"), UrbanistBold);
    }
    void AddResearchToPlayers()
    {
        playerController.researchPoints += 10000f;
        uiController.CreateTextObject("Text", ResearchAnimate, researchColor, new Vector3(0f,40f,0f), "none", ("+ 10,000"), UrbanistBold);
    }
}