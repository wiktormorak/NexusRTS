using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiscList", menuName = "Buildings/MiscList")]
public class MiscList : ScriptableObject
{
    public string groupName;
    public buildingClass BuildingClass;
    public List<Building> miscList;
}