using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    
    private PlayerInput playerInput;
    private InputAction moveAction;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 move = new Vector3(input.x, 0, input.y);
        
        transform.Translate(move * speed * Time.deltaTime);
    }
}