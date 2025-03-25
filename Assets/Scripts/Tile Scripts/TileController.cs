using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class TileController : MonoBehaviour
{
    public GameObject tileManager;
    private PlayerController playerController;
    #region Player Vars
    public GameObject player;  
    private bool playerHeatMaptile;
    private bool playerInConfigModetile;
    private bool playerInBuildModetile;
    #endregion
    #region Tile Vars
    public int seed;
    public List<GameObject> tileGroups;
    public List<GameObject> tiles;
    private GameObject[] tilesArray;
    private int tileCount;
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
    private Color normalColor;
    private TMPro.TMP_FontAsset UrbanistMedium;
    private TMPro.TMP_FontAsset UrbanistBold;
    #endregion
    #region Vegetation Objects
    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;
    #endregion
    #region GameObject Arrays
    public List<GameObject> Tier1Objects;
    public List<GameObject> Tier2Objects;
    public List<GameObject> Tier3Objects;
    public List<GameObject> Tier4Objects;
    public List<GameObject> Tier5Objects;
    public List<GameObject> ForestObjects;
    public List<GameObject> playerOwnedBuildingList;
    #endregion
    #region Renderer Arrays
    private List<Renderer> vegetationRenderers;
    public List<Renderer> allTileRenderers;
    public List<Renderer> Tier1Renderer;
    public List<Renderer> Tier2Renderer;
    public List<Renderer> Tier3Renderer;
    public List<Renderer> Tier4Renderer;
    public List<Renderer> Tier5Renderer;
    public List<Renderer> ForestRenderer;
    private List<Renderer> playerOwnedBuildingRenderers;
    #endregion
    #region NavMesh Related
    public GameObject navSurface;
    private NavMeshData navData;
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
    #region Config Mode Mats
    Material configModeFloor;
    Material configModeBuilding;
    #endregion
    #region Unity Funcs & Other
    void Start()
    {
        Invoke("StoreFonts", 1);
        Invoke("GetMaterials",1);
        Invoke("StoreAllTiles", 1);
        Invoke("GenerateSeed", 1);
        Invoke("AssignPerlinNoiseToTiles", 1);
        Invoke("TierDecider", 1);
        Invoke("CorrectUnassignedTiles", 1);
        Invoke("SetColours", 1);
        Invoke("FindAnimateGameObjects", 1);
        //Invoke("DebugTiles", 1);
        //Invoke("DecideOreMaximumAmount", 1);
        Invoke("PlaceVegetation", 1);
        Invoke("BakeNavSurface", 1);
        Invoke("FindRenderersAllTiles", 1);
        Invoke("FindRendererFromObjectArray",1);
    }
    void Update()
    {
        //GetExternalVars();
        //AllowOreGeneration();
    }
    void StoreFonts()
    {
        UrbanistMedium = Resources.Load<TMP_FontAsset>("Fonts/UrbanistMedium");
        UrbanistBold = Resources.Load<TMP_FontAsset>("Fonts/UrbanistBold");
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
    void GetExternalVars()
    {
        playerHeatMaptile = player.GetComponent<PlayerController>().playerInHeatMap;
        playerInBuildModetile = player.GetComponent<PlayerController>().playerInBuildMode;
        playerOwnedBuildingList = player.GetComponent<PlayerController>().playerOwnedBuildings;
        player.GetComponent<PlayerController>().playerInConfigMode = playerInConfigModetile;
        playerOwnedBuildingRenderers = player.GetComponent<PlayerController>().playerOwnedBuildingsRenderers;
        playerController = player.GetComponent<PlayerController>();
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
        #region Config Mode
        configModeFloor = Resources.Load("configModeFloor", typeof(Material)) as Material;
        configModeBuilding = Resources.Load("configModeBuildings", typeof(Material)) as Material;
        #endregion
    }
    void BakeNavSurface()
    {
        Vector3 navMeshPosition = new Vector3(-154.203f, 1.04f, -125.1085f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        var sources = new List<NavMeshBuildSource>();
        MeshFilter meshFilter = navSurface.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("NavMeshSurface must have a MeshFilter with a valid Mesh!");
            return;
        }
        var src = new NavMeshBuildSource
        {
            shape = NavMeshBuildSourceShape.Mesh,
            sourceObject = meshFilter.sharedMesh, // Assign the mesh
            transform = navSurface.transform.localToWorldMatrix, // Correct transform
            size = Vector3.one // size is not used for Mesh shapes
        };
        sources.Add(src);
        NavMeshBuildSettings navSettings = NavMesh.GetSettingsByID(0);
        if (navSettings.agentRadius == 0f)
        {
            Debug.LogError("Invalid NavMeshBuildSettings. Ensure the agent type ID is correct.");
            return;
        }
        Bounds localBounds = meshFilter.sharedMesh.bounds;
        localBounds.center = navSurface.transform.position;
        navData = NavMeshBuilder.BuildNavMeshData(navSettings, sources, localBounds, navMeshPosition, rotation);
    }
    void StoreAllTileGroups()
    {

    }
    void StoreAllTiles()
    {
        tilesArray = (GameObject.FindGameObjectsWithTag("Tile"));
        tiles = tilesArray.ToList();
    }
    public List<List<GameObject>> GetSurroundingTiles(GameObject startTile, int layersToFind)
    {
        var parentList = new List<List<GameObject>>();
        var layerMulti = 0f;
        for(int i = 0; i < layersToFind; i++)
        {
            var tempList = new List<GameObject>();
            Collider[] hitColliders = Physics.OverlapSphere(startTile.transform.position, (1.25f * layerMulti));
            foreach (var hitCollider in hitColliders)
            {
                tempList.Add(hitCollider.gameObject);
            }
            parentList.Add(tempList);
            layerMulti = layerMulti + 2f;
        }
        return parentList;
    }
    void GenerateSeed()
    {
        seed = Random.Range(0, 9999999);
    }
    #endregion
    #region Tier Decider
    void TierDecider()
    {
        foreach(GameObject tile in tiles)
        {
            var tilePerlin = tile.GetComponent<TileData>().perlinValue;
            if(tilePerlin >= 0.4f && tilePerlin <= 0.5f)
            {
                tile.tag = "tier1Tile";
                Tier1Objects.Add(tile);
            }
            else if(tilePerlin >= 0.5f && tilePerlin <= 0.6f)
            {
                tile.tag = "tier1Tile";
                Tier1Objects.Add(tile);
            }
            else if(tilePerlin >= 0.3f && tilePerlin <= 0.35f)
            {
                tile.tag = "tier1Tile";
                Tier1Objects.Add(tile);
            }
            else if((tilePerlin >= 0.35f && tilePerlin <= 0.4f))
            {
                tile.tag = "tier2Tile";
                Tier2Objects.Add(tile);
            }
            else if(tilePerlin >= 0.6f && tilePerlin <= 0.7f)
            {
                tile.tag = "tier2Tile";
                Tier2Objects.Add(tile);
            }
            else if(tilePerlin >= 0.7f && tilePerlin <= 0.8f)
            {
                tile.tag = "tier3Tile";
                Tier3Objects.Add(tile);
            }
            else if(tilePerlin >= 0.8 && tilePerlin <= 0.875)
            {
                tile.tag = "tier3Tile";
                Tier3Objects.Add(tile);
            }
            else if(tilePerlin >= 0.875f && tilePerlin <= 0.9f)
            {
                tile.tag = "tier4Tile";
                Tier4Objects.Add(tile);
            }
            else if(tilePerlin <= 0.1)
            {
                tile.tag = "tier4Tile";
                Tier4Objects.Add(tile);
            }
            else if(tilePerlin >= 0.9)
            {
                tile.tag = "tier5Tile";
                Tier5Objects.Add(tile);
            }
        }
    }
    void CorrectUnassignedTiles()
    {
        foreach(GameObject tile in tiles)
        {
            if(tile.CompareTag("Untagged"))
            {
                tile.tag = "tier1Tile";
                Tier1Objects.Add(tile);
            }
        }
    }
    #endregion
    #region Vegetation Gen
    void PlaceVegetation()
    {
        foreach(GameObject tile in tiles)
        {
            var tilePerlin = tile.GetComponent<TileData>().perlinValue;
            if(tilePerlin >= 0.2f && tilePerlin <= 0.32f)
            {
                tile.tag = "forestTile";
                ForestObjects.Add(tile);
                //forestTile.tag = ("hasTree");
                var treeObject = Instantiate(tree1);
                treeObject.isStatic = true;
                var pos = tile.transform.position;
                treeObject.transform.position = pos;
                treeObject.transform.SetParent(tile.transform);
                // foreach(Renderer renderer in treeObject)
                // {
                //     vegetationRenderers.Add(renderer);
                // }
            }
            else if(tilePerlin >= 0.32f && tilePerlin <= 0.36f)
            {
                tile.tag = "forestTile";
                ForestObjects.Add(tile);
                //forestTile.tag = ("hasTree");
                var treeObject = Instantiate(tree2);
                treeObject.isStatic = true;
                var pos = tile.transform.position;
                treeObject.transform.position = pos;
                treeObject.transform.SetParent(tile.transform);
                // foreach(Renderer renderer in treeObject)
                // {
                //     vegetationRenderers.Add(renderer);
                // }
            }
        }
    }
    void AssignPerlinNoiseToTiles()
    {
        Random.InitState(seed);
        float offsetX = Random.Range(0f, 99999f);
        float offsetZ = Random.Range(0f, 99999f);
        foreach(GameObject tile in tiles)
        {
            var qWorldPos = new Vector2(tile.transform.position.x, tile.transform.position.z);
            var rWorldPos = new Vector2(tile.transform.position.z, tile.transform.position.x);
            var hexRadius = 0.05f;
            var noiseScale = 1f;
            int q = WorldToGridColumn(qWorldPos, hexRadius);
            int r = WorldToGridRow(rWorldPos, hexRadius);
            float x = hexRadius * Mathf.Sqrt(3) * (q + r * 0.5f) + offsetX;
            float z = hexRadius * 1.5f * r + offsetZ;
            tile.GetComponent<TileData>().perlinValue = Mathf.PerlinNoise(x * noiseScale, z * noiseScale);
        }
    }
    public int WorldToGridColumn(Vector2 worldPos, float hexRadius)
    {
        float q = (2f / 3f * worldPos.x) / hexRadius;
        int roundedQ = Mathf.RoundToInt(q);
        return roundedQ;
    }
    public int WorldToGridRow(Vector2 worldPos, float hexRadius)
    {
        float r = (-1f / 3f) * worldPos.x / hexRadius + (Mathf.Sqrt(3f) / 3f) * worldPos.y / hexRadius; 
        int roundedR = Mathf.RoundToInt(r);
        return roundedR;
    }
    #endregion
    #region Renderers from Object Arrays
    void FindRendererFromObjectArray()
    {
        foreach (GameObject tier1 in Tier1Objects)
        {
            Tier1Renderer.Add(tier1.GetComponent<Renderer>());
        }
        foreach (GameObject tier2 in Tier2Objects)
        {
            Tier2Renderer.Add(tier2.GetComponent<Renderer>());
        }
        foreach (GameObject tier3 in Tier3Objects)
        {
            Tier3Renderer.Add(tier3.GetComponent<Renderer>());
        }
        foreach (GameObject tier4 in Tier4Objects)
        {
            Tier4Renderer.Add(tier4.GetComponent<Renderer>());
        }
        foreach (GameObject tier5 in Tier5Objects)
        {
            Tier5Renderer.Add(tier5.GetComponent<Renderer>());
        }
        foreach (GameObject forest in ForestObjects)
        {
            ForestRenderer.Add(forest.GetComponent<Renderer>());
        }
    }
    void FindRenderersAllTiles()
    {
        foreach(GameObject tile in tiles)
        {
            allTileRenderers.Add(tile.GetComponent<Renderer>());
        }
    }
    void FindRendererBuildings()
    {
        foreach (GameObject building in playerOwnedBuildingList)
        {
            playerOwnedBuildingRenderers.Add(building.GetComponent<Renderer>());
        }
    }
    #endregion
    #region Config Mode
    public void EnableConfigMode()
    {
        foreach (Renderer tileRenderer in allTileRenderers)
        {
            tileRenderer.material = configModeFloor;
        }
        // foreach (Renderer vegetationRenderer in vegetationRenderers)
        // {
        //     vegetationRenderer.material = configModeFloor;
        // }
    }
    public void DisableConfigMode()
    {
        foreach (Renderer tileRenderer in allTileRenderers)
        {
            tileRenderer.material = grass;
        }
    }
    #endregion
    #region Ores
    void AllowOreGeneration()
    {
        if(playerController.oreGenPermittedTiles.Count > 0)
        {
            List<GameObject> oreGenPermittedTiles = playerController.oreGenPermittedTiles;
            foreach(GameObject tile in oreGenPermittedTiles)
            {
                TileOreData tileOreData = tile.GetComponent<TileOreData>();
                if(tileOreData.oreGenerate)
                {
                    tileOreData.DecideOres();
                }
            }
        }
    }
    #endregion
}