using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigInfo", menuName = "Config/ConfigInfo")]
public class ConfigInfo : ScriptableObject
{
    [Header("Configure")]
    public GameObject configScriptGameObject;
    [Header("Upgrade")]
    public bool hasUpgradeOptions;
    public int totalUpgradeStages;
    public List<GameObject> buildingUpgradePrefabs;
    [Header("Upgrade - Values To Change")]
    public int value1;
    public int value1cached;
    public int value2;
    public int value2cached;
    public bool value2used;
    public int value3;
    public int value3cached;
    public bool value3used;
}