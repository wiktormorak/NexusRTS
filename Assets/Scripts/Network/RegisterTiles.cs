using FishNet.Object;
using UnityEngine;
using System.Collections.Generic;

public class RegisterTiles : NetworkBehaviour
{
    public void SetRenderersVisible(bool visible, bool force)
    {
        visible = true;
        force = true;
    }
}