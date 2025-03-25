using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierDecider : MonoBehaviour
{
    public GameObject tileManager;
    private TileController tileController;
    private bool hasDecidedTiers = false; // Flag to ensure DecideTier runs only once
    public List<GameObject> tiles;
    private int tileCount;
    #region Chances
    private float tierOne = 60f;
    private float tierTwo = 10f;
    private float tierThree = 8.5f;
    private float forestTile = 20f;
    private float tierFour = 1f;
    //private float tierFive = 0.5f;
    #endregion
    #region Unity Funcs
    void Start()
    {
        tileController = tileManager.GetComponent<TileController>();
        tileCount = transform.childCount;
        StoreAllChildren();
    }

    void Update()
    {
        if (!hasDecidedTiers)
        {
            DecideTier();
            hasDecidedTiers = true; // Mark as done
        }
    }
    #endregion

    void StoreAllChildren()
    {
        for (int i = 0; i < tileCount; i++)
        {
            tiles.Add(transform.GetChild(i).gameObject);
        }
    }

    void DecideTier()
    {
        var tier1List = tileController.Tier1Objects;
        var tier2List = tileController.Tier2Objects;
        var tier3List = tileController.Tier3Objects;
        var tier4List = tileController.Tier4Objects;
        var tier5List = tileController.Tier5Objects;
        var forestList = tileController.ForestObjects;

        foreach (GameObject child in tiles)
        {
            bool selectedTier = false;
            float value = Random.Range(0f, 100f);
            float cumulative = 0f;

            if (!selectedTier)
            {
                if (value <= (cumulative += tierOne))
                {
                    child.tag = "tier1Tile";
                    tier1List.Add(child);
                    selectedTier = true;
                }
                else if (value <= (cumulative += tierTwo))
                {
                    child.tag = "tier2Tile";
                    tier2List.Add(child);
                    selectedTier = true;
                }
                else if (value <= (cumulative += tierThree))
                {
                    child.tag = "tier3Tile";
                    tier3List.Add(child);
                    selectedTier = true;
                }
                else if (value <= (cumulative += forestTile))
                {
                    child.tag = "forestTile";
                    forestList.Add(child);
                    selectedTier = true;
                }
                else if (value <= (cumulative += tierFour))
                {
                    child.tag = "tier4Tile";
                    tier4List.Add(child);
                    selectedTier = true;
                }
                else
                {
                    child.tag = "tier5Tile";
                    tier5List.Add(child);
                    selectedTier = true;
                }
            }
        }
    }
}