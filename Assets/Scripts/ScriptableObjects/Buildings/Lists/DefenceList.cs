using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenceList", menuName = "Buildings/DefenceList")]
public class DefenceList : ScriptableObject
{
    public string groupName;
    public buildingClass BuildingClass;
    public List<Building> defenceList;
}