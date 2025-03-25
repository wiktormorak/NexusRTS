using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Combat/Projectile")]
public class Projectile : ScriptableObject
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

    [Header("Projectile Info")]
    public string projectileName;
    public string projectileDescription;
    //public int buildingCost;
    //public float buildTime;
    public string idName;
    public int projID;

    [Header("Projectile Stats")]
    public int hitChance;
    public int critChance;
    public int baseDamage;
    public float critMultiplier;
    public int splashDamage;
    public float accuracy;
    public bool isExplosive;
    public bool isLongRange;
    public bool isBullet;
    public bool isShell;
}