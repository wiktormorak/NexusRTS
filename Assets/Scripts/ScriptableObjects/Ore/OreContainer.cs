using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OreContainer", menuName = "Ore/OreContainer")]
public class OreContainer : ScriptableObject
{
    public List<Ore> oreTypes;
}