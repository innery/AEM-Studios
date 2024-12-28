using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public float attackRange = 1f; // Sald�r� mesafesi
    public float damagePerSecond = 10f; // Saniyede verilen hasar
    private float damageInterval = 1f; // Hasar uygulama aral���
    private float lastDamageTime = 0f;

    public Transform target; // Hedef karakter (Inspector'dan atanabilir)
    private Character botCharacter; // Bu botun karakter scripti
    private Animator animator; // Botun Animator bile�eni

    void Start()
    {
        botCharacter = GetComponent<Character>();
        animator = GetComponent<Animator>();

        if (botCharacter == null)
        {
            Debug.LogError("Character scripti bot �zerinde bulunamad�!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator bile�eni bot �zerinde bulunamad�!");
        }

        if (target == null)
        {
            Debug.LogError("Hedef atanmad�! Inspector �zerinden bir hedef atay�n.");
        }
    }

    void Update()
    {
        if (target == null || botCharacter == null || botCharacter._health <= 0)
        {
            // Hedef veya bot ge�ersizse i�lem yapma
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // E�er hedef sald�r� menzilindeyse
        if (distanceToTarget <= attackRange)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        // Hasar aral���n� kontrol et
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
                Debug.LogWarning("Hedef karakterde Character scripti bulunamad�!");
            }
        }
    }
}
