using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public NetworkVariable<bool> IsGamePlaying = new NetworkVariable<bool>(false);


    void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        if (!IsServer) return;

        AssignRoles();

        IsGamePlaying.Value = true;
        
        StartGameClientRpc();
    }

    void AssignRoles()
    {
        List<ulong> clientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);

        int randomIndex = Random.Range(0, clientIds.Count);
        ulong hunterId = clientIds[randomIndex];

        foreach (ulong id in clientIds)
        {
            if (id == hunterId)
            {
                AssignRoleClientRpc(PlayerRole.Hunter, new ClientRpcParams
                {
                    Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { id } }
                });
            }
            else
            {
                AssignRoleClientRpc(PlayerRole.Civilian, new ClientRpcParams
                {
                    Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { id } }
                });
            }
        }
    }

    [ClientRpc]
    void AssignRoleClientRpc(PlayerRole role, ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("MY ROLE IS: " + role);
        
        var localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject;
        if (localPlayer != null)
        {
            localPlayer.GetComponent<PlayerState>().currentRole = role;
        }
    }

    [ClientRpc]
    void StartGameClientRpc()
    {
        GameObject lobby = GameObject.Find("Panel_Lobby");
        if (lobby != null) lobby.SetActive(false);
    }
}