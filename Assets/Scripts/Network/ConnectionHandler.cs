using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Transporting;
using FishNet.Connection;

public enum ConnectionType
{
    Client,
    Host
}

public class ConnectionHandler : MonoBehaviour
{
    public GameObject playerSpawnHandler;
    private PlayerSpawnHandler PlayerSpawnHandler;
    public ConnectionType connectionType;
    public Dictionary<NetworkConnection, GameObject> players = new Dictionary<NetworkConnection, GameObject>();
    private NetworkConnection temporaryNetConn;
    void StorePlayerSpawnHandler()
    {
        PlayerSpawnHandler = playerSpawnHandler.GetComponent<PlayerSpawnHandler>();
    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        // temporaryNetConn = InstanceFinder.ClientManager.Connection;
        // players.Remove(temporaryNetConn, temporaryNetConn.FirstObject.gameObject);
    }

    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        if(args.ConnectionState == LocalConnectionState.Stopped)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private void OnPlayerConnected(NetworkConnection conn, RemoteConnectionStateArgs state)
    {
        if (state.ConnectionState == RemoteConnectionState.Started)
        {
           PlayerSpawnHandler.SpawnPlayer(conn);
        }
    }

    private void Start()
    {
        Invoke("StorePlayerSpawnHandler", 1);
        //#if UNITY_EDITOR
        if(ParrelSync.ClonesManager.IsClone())
        {
            InstanceFinder.ClientManager.StartConnection();
        }
        else
        {
            if(connectionType == ConnectionType.Host)
            {
                InstanceFinder.ServerManager.StartConnection();
                InstanceFinder.ClientManager.StartConnection();
                //temporaryNetConn = InstanceFinder.ClientManager.Connection;
                //players.Add(temporaryNetConn, temporaryNetConn.FirstObject.gameObject);
            }
            else
            {
                InstanceFinder.ClientManager.StartConnection();
            }
        }
        //#endif
    }
}
