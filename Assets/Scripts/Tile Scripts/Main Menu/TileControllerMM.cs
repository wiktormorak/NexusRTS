using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControllerMM : MonoBehaviour
{
    public GameObject tileManager;
    #region Tile Vars
    public int seed;
    public List<GameObject> tiles;
    private int tileCount;
    public List<GameObject> ForestObjects;
    #endregion
    #region Vegetation Objects
    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("GenerateSeed", 1);
        Invoke("StoreAllChildren", 1);
        Invoke("AssignPerlinNoiseToTiles", 1);
        Invoke("PlaceVegetation", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateSeed()
    {
        seed = Random.Range(0, 9999999);
    }
    void StoreAllChildren()
    {
        tileCount = transform.childCount;
        for (int i = 0; i < tileCount; i++)
        {
            tiles.Add(transform.GetChild(i).gameObject);
        }
    }
    void PlaceVegetation()
    {
        foreach(GameObject tile in tiles)
        {
            var tilePerlin = tile.GetComponent<TileData>().perlinValue;
            if(tilePerlin >= 0.2f && tilePerlin <= 0.375f)
            {
                tile.tag = "forestTile";
                ForestObjects.Add(tile);
                //forestTile.tag = ("hasTree");
                var treeObject = Instantiate(tree1);
                treeObject.isStatic = true;
                var pos = tile.transform.position;
                treeObject.transform.position = pos;
                treeObject.transform.SetParent(tile.transform);
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
}