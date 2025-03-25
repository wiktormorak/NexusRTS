using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMouseEvents : MonoBehaviour
{
    #region external vars
    public GameObject player;
    public GameObject tileManager;
    private PlayerController playerController;
    private bool tileEventsplayerInConfigMode;
    private bool tileEventsplayerInHeatMap;
    private GameObject tileEventsOnHoverTile;
    #endregion
    #region Hover Mats
    Material onHoverMat;
    Material baseMat;
    Material falseMat;
    #endregion
    #region Heat Map Mats
    Material tier1Mat;
    Material tier2Mat;
    Material tier3Mat;
    Material tier4Mat;
    Material tier5Mat;
    #endregion
    #region config mode mats
    Material configModeFloor;
    Material configModeBuilding;
    #endregion
    #region Unity Funcs
    void Start()
    {
        #region hover mat set
        onHoverMat = Resources.Load("OnHoverMat", typeof(Material)) as Material;
        baseMat = Resources.Load("Grass", typeof(Material)) as Material;
        falseMat = Resources.Load("OnHoverFalseMat", typeof(Material)) as Material;
        #endregion
        #region heat map mats set
        tier1Mat = Resources.Load("Tier1", typeof(Material)) as Material;
        tier2Mat = Resources.Load("Tier2", typeof(Material)) as Material;
        tier3Mat = Resources.Load("Tier3", typeof(Material)) as Material;
        tier4Mat = Resources.Load("Tier4", typeof(Material)) as Material;
        tier5Mat = Resources.Load("Tier5", typeof(Material)) as Material;
        #endregion
        #region config mode mats
        configModeFloor = Resources.Load("configModeFloor", typeof(Material)) as Material;
        configModeBuilding = Resources.Load("ConfigModeBuilding", typeof(Material)) as Material;
        #endregion
    }
    void GetExternalVars()
    {
        playerController = player.GetComponent<PlayerController>();
        tileEventsplayerInConfigMode = playerController.playerInConfigMode;
        tileEventsplayerInHeatMap = playerController.playerInHeatMap;
        //tileEventsOnHoverTile = playerController.playerOnHoverTile;
    }
    void Update()
    {
        GetExternalVars();
    }
    #endregion
    #region OnHover Events
    void OnMouseOver()
    {
        #region Check For Forest Tile
        if(!tileEventsplayerInHeatMap && !tileEventsplayerInConfigMode)
        {
            gameObject.GetComponent<Renderer>().material = onHoverMat;
            if(gameObject.CompareTag("forestTile"))
            {
                gameObject.GetComponent<Renderer>().material = falseMat;
            }
            else if(tileEventsplayerInConfigMode)
            {
                gameObject.GetComponent<Renderer>().material = configModeFloor;
            }
        }
        else if(tileEventsplayerInConfigMode || tileEventsplayerInHeatMap)
        {
            if(tileEventsplayerInConfigMode && gameObject.transform.childCount > 0)
            {
                var building = gameObject.transform.GetChild(0).gameObject;
                if(building.CompareTag("building"))
                {
                    gameObject.GetComponent<Renderer>().material = configModeBuilding;
                }
            }
            return;
        }
        #endregion
    }
    void OnMouseExit()
    {
        if(!tileEventsplayerInHeatMap && !tileEventsplayerInConfigMode)
        {
            gameObject.GetComponent<Renderer>().material = baseMat;
        }
    }
    #endregion
}