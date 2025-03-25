using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombatUnitHandler : MonoBehaviour
{
    #region Stats
    public CombatUnit combatUnitScriptable;
    public Projectile unitProjectile;
    #endregion
    #region Vars
    public List<GameObject> knownCombatUnits;
    #endregion
    #region Unity Funcs
    void Start()
    {
        Invoke("LookForEnemyUnitsCoroutine", 1);
    }
    void Update()
    {
        
    }
    #endregion
    #region Scouting Functions
    IEnumerator LookForEnemyUnitsCoroutine()
    {
        knownCombatUnits = LookForEnemyUnits();
        yield return new WaitForSeconds(0.5f);
    }
    List<GameObject> LookForEnemyUnits()
    {
        var validGameObjects = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 20f);
        foreach(Collider collider in hitColliders)
        {
            if(collider.gameObject.transform.CompareTag("combatUnit"))
            {
                validGameObjects.Add(collider.gameObject);
            }
        }
        return validGameObjects;
    }
    #endregion
    #region Combat Functions
    void Fire(GameObject target, Projectile projectile)
    {
        #region set vars
        //Ray ray = 
        var isHitRandom = Random.Range(0f,1f);
        var isCritRandom = Random.Range(0f,1f);
        var isHit = false;
        var isCrit = true;
        #endregion
        #region decide if hit and crit
        if(isHitRandom >= 0.1f)
        {
            isHit = true;
        }
        if(isCritRandom <= 0.5f)
        {
            isCrit = true;
        }
        #endregion
    }
    #endregion
}