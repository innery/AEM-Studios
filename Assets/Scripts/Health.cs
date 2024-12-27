using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    Ragdoll ragdoll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
    }
    public void TakeDamage(float damage){
        currentHealth -= damage;
        if(currentHealth <= 0.0f){
            Die();
        }
    }
    void Die(){
        ragdoll.ActivateRagdoll();
    }
}
