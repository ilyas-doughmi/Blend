using UnityEngine;
using Unity.Netcode;
using System.Collections;
public class TaskPoint : NetworkBehaviour
{
    public float timeToComplete = 3f;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.red;

    private bool isCompleted = false;
    private float timer = 0f;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = inactiveColor;
    }

    void OnTriggerStay(Collider other)
    {
        if (isCompleted) return;

        if (other.CompareTag("Player"))
        {
            timer += Time.deltaTime;

            Debug.Log($"Hacking... {timer}/{timeToComplete}");

            if (timer >= timeToComplete)
            {
                CompleteTask();
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0f;
            Debug.Log("Hacking Failed - Left Zone");
        }
    }

    void CompleteTask()
    {
        isCompleted = true;
        
        meshRenderer.material.color = activeColor;
        
        Debug.Log("TASK COMPLETED!");
    }

    

}
