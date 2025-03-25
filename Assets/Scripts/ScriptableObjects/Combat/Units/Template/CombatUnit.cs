using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatUnit", menuName = "Combat/Unit")]
public class CombatUnit : ScriptableObject
{
    [Header("Prefab Data")]
    public GameObject prefab;
    public List<GameObject> prefabList;
    public bool usePrefabLists;
    public float prefabYOffset;
    public float prefabScaleX;
    public float prefabScaleY;
    public float prefabScaleZ;
    public Quaternion prefabRotation;

    [Header("Unit Info")]
    public string displayName;
    public string unitName;
    public string unitDescription;
    public unitType unitType;
    public int buildingCost;
    public float buildTime;
    public string idName;
    public int unitID;

    [Header("Unit Stats")]
    public int health;
    public int armour;
    public int speed;
    public int turnSpeed;
    public Projectile UnitProjectile;
    public int baseDamage;
    public float critMultiplier;
    public int splashDamage;
    public float accuracy;
    public float scoutingCooldown;
    public List<Projectile> compatibleProjectiles;
}

public enum unitType
{
    groundVehicle,
    groundPersonnel,
    air,
    water
}