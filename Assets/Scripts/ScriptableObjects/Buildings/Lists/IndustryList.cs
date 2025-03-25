using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IndustryList", menuName = "Buildings/IndustryList")]
public class IndustryList : ScriptableObject
{
    public string groupName;
    public buildingClass BuildingClass;
    public List<Building> industryList;
}
