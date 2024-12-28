using UnityEngine;
using UnityEngine.AI;

public class AILocomation : MonoBehaviour
{
    public Transform playerTransform;
    public NavMeshAgent agent;
    private Animator animator;

    // Character script'ine referans
    private Character characterScript;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Ayný GameObject'teki Character script'ine referans al
        characterScript = GetComponent<Character>();

        if (characterScript == null)
        {
            Debug.LogError("Character script bulunamadý! Doðru script'in GameObject'e ekli olduðundan emin olun.");
        }
    }

    void Update()
    {
        // Eðer health sýfýrsa hareketi durdur
        if (characterScript != null && characterScript._health <= 0)
        {
            agent.isStopped = true; // NavMeshAgent'i durdur
            animator.SetFloat("Speed", 0); // Animasyonu durdur
            return; // Diðer kodlarýn çalýþmasýný engelle
        }

        // Botun hedefe hareket etmesi
        agent.destination = playerTransform.position;
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
