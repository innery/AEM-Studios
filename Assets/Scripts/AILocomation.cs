using UnityEngine;
using UnityEngine.AI;

public class AILocomation : MonoBehaviour
{
    public Transform playerTransform;
    public NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.destination = playerTransform.position;
    }    
}
