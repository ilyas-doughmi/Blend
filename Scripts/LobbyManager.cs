using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Collections.Generic;
using Unity.Networking.Transport.Relay; 

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    
    private Lobby currentLobby;
    private float heartbeatTimer;

    async void Start()
    {
        Instance = this;
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
    }

    void Update()
    {
        HandleLobbyHeartbeat();
    }


    public async void CreateLobby(string lobbyName)
    {
        try
        {

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(10);
            
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            CreateLobbyOptions options = new CreateLobbyOptions();
            options.Data = new Dictionary<string, DataObject>
            {
                { "RelayCode", new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) } 
            };

            currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 10, options);
            Debug.Log("Created Lobby: " + lobbyName + " | Code: " + currentLobby.LobbyCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
            
            NetworkManager.Singleton.StartHost();
            
            NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }


    public async void JoinLobby(string lobbyId)
    {
        try
        {
            currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

            string relayJoinCode = currentLobby.Data["RelayCode"].Value;

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            NetworkManager.Singleton.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }
    
    
    public async void RefreshLobbyList()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 20;

            QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync(options);

            Debug.Log("Found " + response.Results.Count + " lobbies.");
            
            MainMenuUI.Instance.UpdateLobbyList(response.Results);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private async void HandleLobbyHeartbeat()
    {
        if (currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                heartbeatTimer = 15f; 
                await LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
            }
        }
    }
}