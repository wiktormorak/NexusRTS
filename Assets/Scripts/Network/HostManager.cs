using FishNet.Managing;
using FishNet.Transporting.UTP;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using TMPro;
public class HostManager : MonoBehaviour
{
    [Header("Assignables")]
    public NetworkManager _networkManager;
    public string joinText;
    public GameObject RelayMenu;
    public TMP_Text LobbyIDText;
    
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);

        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    
    public async void StartHost()
    {
        var utp = (FishyUnityTransport)_networkManager.TransportManager.Transport;

        Allocation hostAllocation = await RelayService.Instance.CreateAllocationAsync(4);
        utp.SetRelayServerData(new RelayServerData(hostAllocation, "dtls"));
        string joinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(hostAllocation.AllocationId);
        Debug.Log(joinCode);

        _networkManager.ServerManager.StartConnection();
        _networkManager.ClientManager.StartConnection();
        RelayMenu.SetActive(false);
        LobbyIDText.text = joinCode;
    }

    public async void StartClient(string joinCode)
    {
        var utp = (FishyUnityTransport)_networkManager.TransportManager.Transport;
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        utp.SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        Debug.Log(joinCode + " is the joinCode");
        _networkManager.ClientManager.StartConnection();
        RelayMenu.SetActive(false);
    }
}