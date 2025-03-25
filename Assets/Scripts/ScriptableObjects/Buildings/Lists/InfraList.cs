using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfraList", menuName = "Buildings/InfraList")]
public class InfraList : ScriptableObject
{
    public string groupName;
    public buildingClass BuildingClass;
    public List<Building> infraList;
}