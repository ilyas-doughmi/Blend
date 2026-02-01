using UnityEngine;
using Unity.Netcode;

public class GameReferee : NetworkBehaviour
{
    public NetworkVariable<int> tasksCompleted = new NetworkVariable<int>(0);
    public int totalTasksNeeded = 3;

    public static GameReferee Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ReportTaskDone()
    {
        if (IsServer)
        {
            tasksCompleted.Value++;
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
        if (tasksCompleted.Value >= totalTasksNeeded)
        {
            Debug.Log("HIDERS WIN! 🏆");
            ShowGameOverClientRpc("HIDERS WIN!");
        }
    }

    [ClientRpc]
    void ShowGameOverClientRpc(string message)
    {
        Debug.Log("GAME OVER: " + message);
    }
}