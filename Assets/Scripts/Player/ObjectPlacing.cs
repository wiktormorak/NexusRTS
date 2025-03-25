using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using FishNet.Object;
using FishNet.Connection;

public class ObjectPlacing : MonoBehaviour
{
    public GameObject playerObject;
    private Player playerController;
    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitDictionary()
    {
        playerController.categoryMapping = new Dictionary<string, ScriptableObject>
        {
            { "industry", playerController.industrySO },
            { "defence", playerController.defenceSO },
            { "infra", playerController.infraSO },
            { "misc", playerController.miscSO }
        };
    }
    void UpdateCurrentCategory()
    {
        playerController.build_currentCategory =  playerController.uiController.playerCategory;
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
        playerController.build_currentlySelected = parentName;
    }
    public GameObject FindBuildingByName()
    {
        #region list
        var temporary = new List<List<Building>>()
        {
            playerController.industryList,
            playerController.defenceList,
            playerController.infraList,
            playerController.miscList
        };
        #endregion
        #region Store Data
        void Store(Building listBuilding)
        {
            playerController.build_toPlace = listBuilding.prefab;
            playerController.build_idName = listBuilding.idName;
            playerController.build_displayName = listBuilding.displayName;
            playerController.build_buildingPrice = listBuilding.buildingCost;
            playerController.build_prefabOffset = listBuilding.prefabOffset;
            playerController.build_prefabScale = listBuilding.prefabScale;
            playerController.build_prefabRotation = listBuilding.prefabRotation;
            playerController.build_doesObjectUsePrefabList = listBuilding.usePrefabLists;
            if(playerController.build_doesObjectUsePrefabList)
            {
                playerController.build_objectPrefabList = listBuilding.prefabList;
                playerController.build_toPlace = playerController.build_objectPrefabList[Random.Range(0, playerController.build_objectPrefabList.Count)];
            }
            playerController.build_buildTime = listBuilding.buildTime;
            playerController.build_progressBarPrefabX = listBuilding.progressBarPrefabX;
            playerController.build_progressBarPrefabY = listBuilding.progressBarPrefabY;
            playerController.build_progressBarPrefabZ = listBuilding.progressBarPrefabZ;
            playerController.build_progressBarPrefabScale = listBuilding.progressBarPrefabScale;
        }
        #endregion
        #region Find Building By Name
        if (playerController.categoryMapping.TryGetValue(playerController.build_currentCategory, out var categorySO))
        {
            #region industry list
            if (categorySO is IndustryList industrySO)
            {
                foreach (var building in temporary[0])
                {
                    if (building != null && building.idName == playerController.build_currentlySelected)
                    {
                        Store(building);
                    }
                }
                return playerController.build_toPlace;
            }
            #endregion
            #region defense list
            else if (categorySO is DefenceList defenceSO)
            {
                foreach (var building in temporary[1])
                {
                    if (building != null && building.idName == playerController.build_currentlySelected)
                    {
                        Store(building);
                    }
                }
                return playerController.build_toPlace;
            }
            #endregion
            #region infra list
            else if (categorySO is InfraList infraSO)
            {
                foreach (var building in temporary[2])
                {
                    if (building != null && building.idName == playerController.build_currentlySelected)
                    {
                        Store(building);
                    }
                }
                return playerController.build_toPlace;
            }
            #endregion
            #region misc list
            else if (categorySO is MiscList miscSO)
            {
                foreach (var building in temporary[3])
                {
                    if (building != null && building.idName == playerController.build_currentlySelected)
                    {
                        Store(building);
                    }
                }
                return playerController.build_toPlace;
            }
            #endregion
        }
        return playerController.build_toPlace;
        #endregion
    }    
    public void PlaceSelected(NetworkConnection target)
    {
        if(Input.GetMouseButtonDown(0) && !string.IsNullOrEmpty(playerController.build_currentlySelected) && playerController.playerInBuildMode && playerController.playerOnHoverTile.transform.childCount == 0)
        {   
            var selectedPrefab = FindBuildingByName();
            GameObject placedObject = transform.gameObject;
            if(playerController.build_buildingPrice <= playerController.money)
            {
                #region Instaniate
                placedObject = Instantiate(selectedPrefab);
                #endregion
                #region Set PlacedObject Tags
                placedObject.transform.SetParent(playerController.playerOnHoverTile.transform);
                placedObject.name = playerController.build_idName;
                placedObject.tag = "building";
                placedObject.transform.position = new Vector3(playerController.playerOnHoverTile.transform.position.x + playerController.build_prefabOffset.x, 
                playerController.playerOnHoverTile.transform.position.y + playerController.build_prefabOffset.y, playerController.playerOnHoverTile.transform.position.z + playerController.build_prefabOffset.z);
                placedObject.transform.localScale = new Vector3(playerController.build_prefabScale.x, playerController.build_prefabScale.y, playerController.build_prefabScale.z);
                placedObject.transform.localRotation = Quaternion.Euler(playerController.build_prefabRotation.x, playerController.build_prefabRotation.y, playerController.build_prefabRotation.z);
                #endregion
                #region Progress Bar
                playerController.build_cachedTile = playerController.playerOnHoverTile;
                playerController.build_cachedBuilding = placedObject;
                CreateProgressBar(playerObject, playerController.build_buildTime, playerController.build_cachedTile, playerController.build_cachedBuilding);
                #endregion
                #region Remove Money
                playerController.money -= playerController.build_buildingPrice;
                playerController.uiController.CreateTextObject("Text", playerController.MoneyAnimate, playerController.moneyColor, new Vector3(0f,40f,0f), "negative", ("-" + playerController.build_buildingPrice.ToString()), playerController.UrbanistBold);
                #endregion
                #region Multi-Build
                if(!playerController.playerInMultiBuild)
                {
                    playerController.build_currentlySelected = ("");
                    playerController.build_buildingPrice = 0f;
                }
                #endregion
            }
            else if(playerController.build_buildingPrice > playerController.money)
            {
                playerController.uiController.CreateTextObject("Text", playerController.BottomAnimate, playerController.negativeColor, new Vector3(0f,60f,0f), "negative", ("Not enough money to build a " + playerController.build_displayName), playerController.UrbanistMedium);
                playerController.build_currentlySelected = ("");
                playerController.build_buildingPrice = 0f;
            }
        }
    }
    private void InstaniateBuilding(GameObject building)
    {

    }
    private void RemoveMoney()
    {
        
    }
    #endregion
    #region Object Placing Utility Functions
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
        playerController.civilianAmount = playerController.civilianAmount + civiliansToAdd;
    }
    public void AddHouses(int housesToAdd)
    {
        playerController.houseAmount = playerController.houseAmount + housesToAdd;
    }
    public void AddToOwnedBuildings(GameObject building)
    {
        playerController.playerOwnedBuildings.Add(building);
        playerController.playerOwnedBuildingsRenderers.Add(building.GetComponent<Renderer>());
    }
    #endregion
}