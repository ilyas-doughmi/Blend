using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class LobbyUI : NetworkBehaviour
{
    public Button startButton;
    public GameObject panelObject;

    void Start()
    {
        startButton.gameObject.SetActive(false);

        startButton.onClick.AddListener(() =>
        {
            if (IsHost)
            {
                GameManager.Instance.StartGame();
                HideLobbyServerRpc();
            }
        });
    }

    void Update()
    {
        if (IsHost)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void HideLobbyServerRpc()
    {
        HideLobbyClientRpc();
    }

    [ClientRpc]
    void HideLobbyClientRpc()
    {
        panelObject.SetActive(false);
    }
}
