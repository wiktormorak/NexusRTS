using UnityEngine;

[CreateAssetMenu(fileName = "BuildingExtraInfo", menuName = "Buildings/BuildingExtraInfo")]
public class BuildingExtraInfo : ScriptableObject
{
    public string stringValue;
    public stringUse stringValueUse;
    public int integerValue;
    public integerUse integerValueUse;
    public bool boolValue;
    public boolUse boolValueUse;
    public GameObject secondaryPrefab;
    public GameObject teritaryPrefab;
}
public enum stringUse
{
    none,
    name,
    description,
    id,
    misc
}
public enum integerUse
{
    none,
    price,
    chance,
    percentage,
    misc,

}
public enum boolUse
{
    none,
    requirement,
    misc,
}