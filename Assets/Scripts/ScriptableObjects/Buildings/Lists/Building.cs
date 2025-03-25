using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Building", menuName = "Buildings/Building")]
public class Building : ScriptableObject
{
    [Header("UI Prefabs - Progress Bar")]
    public float progressBarPrefabX;
    public float progressBarPrefabY;
    public float progressBarPrefabZ;
    public float progressBarPrefabScale;

    [Header("UI Prefabs - Config")]
    public ConfigInfo configInfo;

    [Header("Prefab Data")]
    public GameObject prefab;
    public List<GameObject> prefabList;
    public List<GameObject> upgradePrefabList;
    public bool usePrefabLists;
    public Vector3 prefabOffset;
    public Vector3 prefabScale;
    public Vector3 prefabRotation;

    [Header("Extras")]
    public bool requiresSecondButton;
    public buttonUse requiresSecondButtonUse;
    public bool requiresExtraDetail;
    public List<ExtraConfigData> buildingExtrasList;

    [Header("Building Info")]
    public string buildingName {get;}
    public string buildingDescription;
    public buildingClass BuildingClass;
    public int buildingCost;
    public float buildTime;
    public int upgradeStage;
    public string displayName;
    public string idName;
    public int buildingID;
    public buttonUse firstButtonUse;
}

public enum buildingClass
{
    defence,
    industry,
    infrastructure,
    misc
}

public enum buttonUse
{
    none,
    upgrade,
    configure,
    other,
}