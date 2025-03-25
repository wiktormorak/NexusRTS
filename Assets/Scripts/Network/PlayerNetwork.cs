using FishNet.Object;
using FishNet.Connection;
using FishNet.Transporting;
using UnityEngine;
using System.Collections.Generic;

public class PlayerNetwork : NetworkBehaviour
{
    public GameObject playerSpawnHandler;
    private PlayerSpawnHandler PlayerSpawnHandler;
    private static PlayerNetwork instance;
    public Dictionary<NetworkConnection, GameObject> players = new Dictionary<NetworkConnection, GameObject>();
    void Start()
    {
        StorePlayerSpawnHandler();
    }
    void StorePlayerSpawnHandler()
    {
        PlayerSpawnHandler = playerSpawnHandler.GetComponent<PlayerSpawnHandler>();
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkManager.GetComponent<NetworkObject>().Spawn(NetworkManager.transform.gameObject);
        NetworkManager.ServerManager.OnRemoteConnectionState += OnPlayerConnected;
    }

    private void OnPlayerConnected(NetworkConnection conn, RemoteConnectionStateArgs state)
    {
        if (object.Equals(state, RemoteConnectionState.Started))
        {
            if (conn.FirstObject != null)
            {
                players[conn] = conn.FirstObject.gameObject;
                Debug.Log($"Player {conn.ClientId} stored successfully.");
            }
        }
        else if (object.Equals(state, RemoteConnectionState.Stopped))
        {
            if (players.ContainsKey(conn))
            {
                players.Remove(conn);
                Debug.Log($"Player {conn.ClientId} removed from dictionary.");
            }
        }
    }

    public GameObject GetPlayer(NetworkConnection conn)
    {
        return players.TryGetValue(conn, out GameObject player) ? player : null;
    }
}