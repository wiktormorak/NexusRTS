using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOreData : MonoBehaviour
{
    public bool oreGenerate;
    public bool alreadyGenerated;
    #region data
    public int totalAmount;
    public int maxOres;
    public int maxTotalOres;
    public OreContainer allOres;
    public OreContainer commonOres;
    public OreContainer uncommonOres;
    public OreContainer rareOres;
    public OreContainer ultraRareOres;
    public Ore ore1;
    public int ore1Amount;
    public string ore1Rarity;
    private bool ore1Calculated;
    public Ore ore2;
    public int ore2Amount;
    public string ore2Rarity;
    private bool ore2Calculated;
    public Ore ore3;
    public int ore3Amount;
    public string ore3Rarity;
    private bool ore3Calculated;
    public Ore ore4;
    public int ore4Amount;
    public string ore4Rarity;
    private bool ore4Calculated;
    #endregion
    #region Unity Methods
    void Start()
    {
        maxTotalOres = 500000;
    }
    void Update()
    {
        
    }
    #endregion
    #region Ore Methods
    void DecideOreMaximumAmount()
    {
        maxOres = Random.Range(2, 5);
    }
    public void DecideOres()
    {
        if(!alreadyGenerated)
        {
            List<Ore> oreType = allOres.oreTypes;
            ore1 = oreType[Random.Range(0, oreType.Count)];
            ore2 = oreType[Random.Range(0, oreType.Count)];
            ore3 = oreType[Random.Range(0, oreType.Count)];
            ore4 = oreType[Random.Range(0, oreType.Count)];
            SelectUnique();
            ore1Rarity = ore1.oreRarity.ToString();
            ore2Rarity = ore2.oreRarity.ToString();
            ore3Rarity = ore3.oreRarity.ToString();
            ore4Rarity = ore4.oreRarity.ToString();
            DecideAmount();
            alreadyGenerated = true;
        }
    }
    void DecideAmount()
    {
        int remaining = maxTotalOres;
        int[] oreAmounts = new int[4];

        for (int i = 0; i < 3; i++)
        {
            oreAmounts[i] = Random.Range(1, remaining - (3 - i));
            remaining -= oreAmounts[i];
        }
        oreAmounts[3] = remaining;

        for (int i = 0; i < oreAmounts.Length; i++)
        {
            int swapIndex = Random.Range(0, oreAmounts.Length);
            (oreAmounts[i], oreAmounts[swapIndex]) = (oreAmounts[swapIndex], oreAmounts[i]);
        }

        ore1Amount = oreAmounts[0];
        ore2Amount = oreAmounts[1];
        ore3Amount = oreAmounts[2];
        ore4Amount = oreAmounts[3];
    }
    void SelectUnique()
    {
        List<Ore> oreType = allOres.oreTypes;
        do { ore2 = oreType[Random.Range(0, oreType.Count)]; } 
        while (ore2 == ore1);

        do { ore3 = oreType[Random.Range(0, oreType.Count)]; } 
        while (ore3 == ore1 || ore3 == ore2);

        do { ore4 = oreType[Random.Range(0, oreType.Count)]; } 
        while (ore4 == ore1 || ore4 == ore2 || ore4 == ore3);
    }
    #endregion
}