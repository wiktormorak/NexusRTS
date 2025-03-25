using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    #region External
    private GameObject player;
    private PlayerController playerController;
    #endregion
    #region Selected Vehicle & Projectile
    public CombatUnit selectedUnit;
    public GameObject selectedUnitPrefab;
    public Projectile selectedProjectile;
    public List<GameObject> spawnQueue;
    private bool queueEnabled;
    private string vspawn_selectedVehicle;
    #endregion
    #region Unit & Projectile Data
    private float unit_buildCost;
    private float unit_buildTime;
    #endregion
    #region Unity Funcs
    void Start()
    {
        Invoke("ExternalVars", 1);
    }
    void Update()
    {
        
    }
    void ExternalVarsStart()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }
    void ExternalVarsUpdate()
    {
        vspawn_selectedVehicle = playerController.vspawn_selectedVehicle;
    }
    #endregion
    #region yeah
    public void AddVehicleToQueue()
    {
        //vspawn_selectedVehicle;
        spawnQueue.Add(selectedUnitPrefab);
        if(!queueEnabled)
        {
            queueEnabled = true;
        }
    }
    IEnumerator SpawnVehicleCoroutine()
    {
        if(queueEnabled)
        {
            SpawnVehicle();
        }
        yield return null;
    }
    void SpawnVehicle()
    {
        var vehicle = Instantiate(selectedUnitPrefab);
        vehicle.AddComponent<CombatUnitHandler>();
        vehicle.GetComponent<CombatUnitHandler>().combatUnitScriptable = selectedUnit;
        vehicle.GetComponent<CombatUnitHandler>().unitProjectile = selectedProjectile;
    }
    #endregion
}