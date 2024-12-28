using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public float attackRange = 1f; // Saldýrý mesafesi
    public float damagePerSecond = 10f; // Saniyede verilen hasar
    private float damageInterval = 1f; // Hasar uygulama aralýðý
    private float lastDamageTime = 0f;

    public Transform target; // Hedef karakter (Inspector'dan atanabilir)
    private Character botCharacter; // Bu botun karakter scripti
    private Animator animator; // Botun Animator bileþeni

    void Start()
    {
        botCharacter = GetComponent<Character>();
        animator = GetComponent<Animator>();

        if (botCharacter == null)
        {
            Debug.LogError("Character scripti bot üzerinde bulunamadý!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator bileþeni bot üzerinde bulunamadý!");
        }

        if (target == null)
        {
            Debug.LogError("Hedef atanmadý! Inspector üzerinden bir hedef atayýn.");
        }
    }

    void Update()
    {
        if (target == null || botCharacter == null || botCharacter._health <= 0)
        {
            // Hedef veya bot geçersizse iþlem yapma
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Eðer hedef saldýrý menzilindeyse
        if (distanceToTarget <= attackRange)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        // Hasar aralýðýný kontrol et
        if (Time.time - lastDamageTime >= damageInterval)
        {
            lastDamageTime = Time.time;

            // Yumruk atma animasyonunu tetikle
            animator.SetTrigger("Punch");

            // Hedefte Character scripti varsa hasar uygula
            if (target.TryGetComponent(out Character targetCharacter))
            {
                targetCharacter.ApplyDamage(botCharacter, transform, damagePerSecond);
                Debug.Log($"Hedefe {damagePerSecond} hasar verildi. Kalan can: {targetCharacter._health}");
            }
            else
            {
                Debug.LogWarning("Hedef karakterde Character scripti bulunamadý!");
            }
        }
    }
}
