using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI; 

public class NetworkMenu : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;

    void Start()
    {
        
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            HideButtons(); 
        });

        joinButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            HideButtons();
        });
    }

    void HideButtons()
    {
        gameObject.SetActive(false); 
    }
}