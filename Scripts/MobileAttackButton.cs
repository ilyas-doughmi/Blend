using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode; 

public class MobileAttackButton : MonoBehaviour
{
    private Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(OnTap);
    }

    void OnTap()
    {
    
        var myPlayerObject = NetworkManager.Singleton.LocalClient.PlayerObject;
        
        if (myPlayerObject != null)
        {
            myPlayerObject.GetComponent<PlayerAttack>().PerformAttack();
        }
    }
}