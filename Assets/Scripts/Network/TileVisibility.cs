using FishNet.Object;
using FishNet.Connection;
using UnityEngine;
using System.Collections.Generic;

public class TileVisibility : NetworkBehaviour
{
    void Start()
    {
        Invoke("debug", 2);
    }
    void debug()
    {
        Debug.Log(IsServer);
        Debug.Log($"Tile spawned. Observers: {NetworkObject.Observers.Count}");
    }
}
