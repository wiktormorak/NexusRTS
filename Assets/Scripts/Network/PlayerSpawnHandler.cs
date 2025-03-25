using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

public class PlayerSpawnHandler : MonoBehaviour
{
    #region vars
    private GameObject tileManager;
    private TileController tileController;
    public NetworkObject playerPrefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;
    private NetworkManager _networkManager;
    #endregion

    private void Start()
    {
        Invoke("OnStart", 1);
        Invoke("GetSpawnTile", 3);
    }
    void OnStart()
    {
        tileManager = GameObject.FindWithTag("tileManager");
        tileController = tileManager.GetComponent<TileController>();
        _networkManager = FindObjectOfType<NetworkManager>();
        _networkManager.ServerManager.OnRemoteConnectionState += OnPlayerConnected;
        if(tileController.tiles.Count != 0)
        {
            spawnPoint1 = GetSpawnTile();
            spawnPoint2 = GetSpawnTile();
            spawnPoint3 = GetSpawnTile();
            spawnPoint4 = GetSpawnTile();
        }
    }
    private Transform GetSpawnTile()
    {
        int random = Random.Range(0, tileController.tiles.Count);
        Transform spawnPoint = tileController.tiles[random].GetComponent<Transform>();
        return spawnPoint;
    }
    public void SpawnPlayer(NetworkConnection conn)
    {
        if (conn == null || playerPrefab == null) return;
        NetworkObject playerInstance = Instantiate(playerPrefab, spawnPoint1.position, spawnPoint1.rotation);
        _networkManager.ServerManager.Spawn(playerInstance, conn);
    }
    private void OnPlayerConnected(NetworkConnection conn, RemoteConnectionStateArgs state)
    {
        if (state.ConnectionState == RemoteConnectionState.Started)
        {
           SpawnPlayer(conn);
        }
    }
}
