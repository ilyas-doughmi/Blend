using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem; // Needed for the new system

public class PlayerAttack : NetworkBehaviour
{
    public float attackRange = 2f; 
    public LayerMask targetLayer;

    private PlayerInput playerInput;
    private InputAction attackAction;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        playerInput = GetComponent<PlayerInput>();
        
        attackAction = playerInput.actions["Attack"];
    }

    void Update()
    {
        if (!IsOwner) return;

        if (attackAction.WasPerformedThisFrame())
        {
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        Debug.Log("Swinging fist!");
        RequestAttackServerRpc();
    }

    [ServerRpc]
    void RequestAttackServerRpc()
    {
        Vector3 center = transform.position + transform.forward;
        Collider[] hitColliders = Physics.OverlapSphere(center, attackRange, targetLayer);

        foreach (var hit in hitColliders)
        {
            if (hit.gameObject == gameObject) continue;

            if (hit.TryGetComponent<NetworkObject>(out NetworkObject netObj))
            {
                netObj.Despawn(); 
                Debug.Log("Hit something!");
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, attackRange);
    }
}