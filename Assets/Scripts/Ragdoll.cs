using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    public void DeactivateRagdoll(){
        foreach(Rigidbody rigidbody in rigidbodies){
            rigidbody.isKinematic = true;
        }
        animator.enabled = true;
    }
    public void ActivateRagdoll(){
        foreach(Rigidbody rigidbody in rigidbodies){
            rigidbody.isKinematic = false;
        }
        animator.enabled = false;
    }

    // Update is called once per frame
   
}
