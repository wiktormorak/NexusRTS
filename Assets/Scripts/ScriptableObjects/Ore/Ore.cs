using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ore", menuName = "Ore/Ore")]
public class Ore : ScriptableObject
{
    public string oreName;
    public string oreDescription;
    public oreRarity oreRarity;
    public int oreValue;
    

    public int oreChance; 
}

public enum oreRarity
{
    common,
    uncommon,
    rare,
    ultraRare,
}