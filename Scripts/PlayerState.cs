using UnityEngine;
using Unity.Netcode;

public class PlayerState : NetworkBehaviour
{
    public PlayerRole currentRole = PlayerRole.None;
    public Material hunterMat;
    public Material civMat;

    void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("I am a: " + currentRole);
        }
    }
    
}