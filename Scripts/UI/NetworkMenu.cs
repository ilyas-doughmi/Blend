using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkMenu : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;

    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        joinButton.onClick.AddListener(StartClient);
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        HideButtons();
    }

    void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        HideButtons();
    }

    void HideButtons()
    {
        hostButton.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
    }
}
