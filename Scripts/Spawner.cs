using UnityEngine;
using Unity.Netcode;

public class Spawner : NetworkBehaviour
{
    public GameObject botPrefab; 
    public int botCount = 50;    
    public float range = 20f;    

    public override void OnNetworkSpawn()
    {
     
        if (IsServer)
        {
            SpawnCrowd();
        }
    }

    void SpawnCrowd()
    {
        for (int i = 0; i < botCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-range, range), 
                1f, 
                Random.Range(-range, range)
            );

            GameObject bot = Instantiate(botPrefab, randomPos, Quaternion.identity);

            bot.GetComponent<NetworkObject>().Spawn();
        }
    }
}