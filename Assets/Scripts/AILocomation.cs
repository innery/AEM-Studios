using UnityEngine;
using UnityEngine.AI;

public class AILocomation : MonoBehaviour
{
    public Transform playerTransform;
    public NavMeshAgent agent;
    private Animator animator;

    // Character script'ine referans
    private Character characterScript;

    // Bot hareketi için parametreler
    public float randomMoveRadius = 10f; // Rastgele hareket yarýçapý
    public float detectionRadius = 20f; // Oyuncuyu algýlama mesafesi
    public float randomMoveCooldown = 3f; // Rastgele hareket sýklýðý
    private float lastMoveTime;

    private Vector3 randomDestination;

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

        SetRandomDestination();
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

        // Oyuncuya olan mesafeyi hesapla
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Oyuncuya doðru hareket et
            agent.destination = playerTransform.position;
        }
        else
        {
            // Rastgele hareket et
            if (Time.time - lastMoveTime >= randomMoveCooldown || agent.remainingDistance < 0.5f)
            {
                SetRandomDestination();
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomMoveRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, randomMoveRadius, NavMesh.AllAreas))
        {
            randomDestination = navHit.position;
            agent.SetDestination(randomDestination);
        }
        lastMoveTime = Time.time;
    }
}