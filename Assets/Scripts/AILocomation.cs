using UnityEngine;
using UnityEngine.AI;

public class AILocomation : MonoBehaviour
{
    public Transform playerTransform;
    public NavMeshAgent agent;
    Animator animator;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        agent.destination = playerTransform.position;
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }    
}
