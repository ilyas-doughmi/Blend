using UnityEngine;
using UnityEngine.AI; 
using Unity.Netcode;

public class BotMovement : NetworkBehaviour
{
    private NavMeshAgent agent;
    public float range = 20f; 

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return; 

        agent = GetComponent<NavMeshAgent>();
        MoveToRandomPoint();
    }

    void Update()
    {
        if (!IsServer) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToRandomPoint();
        }
    }

    void MoveToRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * range;
        randomPoint += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}