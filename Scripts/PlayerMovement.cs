using UnityEngine;
using UnityEngine.InputSystem; 
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    
    private PlayerInput playerInput;
    private InputAction moveAction;


    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            return;
        }

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];

        Camera.main.GetComponent<CameraFollow>().target = transform;
    }
    

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 move = new Vector3(input.x, 0, input.y);
        
        transform.Translate(move * speed * Time.deltaTime);
    }
}