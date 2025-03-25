using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Squadron", menuName = "Combat/Squadron")]
public class Squadron : ScriptableObject
{
    public string squadronName;
    public List<CombatUnit> allCombatUnits;
    public Building parentBuilding;
    public int unitID;
}