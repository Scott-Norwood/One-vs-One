using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    Rigidbody[] ragdollRigidbodies;
    Rigidbody parentRigidBody;
    Animator animator;
    Enemy_AI enemy_AI;


    void Start()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        parentRigidBody = GetComponentInParent<Rigidbody>();
        animator = GetComponentInParent<Animator>();
        enemy_AI = GetComponentInParent<Enemy_AI>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
            parentRigidBody.isKinematic = false;
        }
        animator.enabled = true;
    }
    public void ActivateRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
            parentRigidBody.isKinematic = false;
        }
        animator.enabled = false;
    }

    public void DeactivateGravity()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
            parentRigidBody.useGravity = false;
            enemy_AI.enabled = false;
        }
        animator.enabled = false;
        StartCoroutine(ReinstateGravity());
    }
    bool isGravityActive;
    public void ActivateGravity()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.useGravity = true;
            parentRigidBody.useGravity = true;
            if (isGravityActive) { rigidbody.isKinematic = true; }
        }
    }

    IEnumerator ReinstateGravity()
    {
        yield return new WaitForSeconds(2f);
        ActivateGravity();
        yield return new WaitForSeconds(3f);
        animator.enabled = true;
        enemy_AI.enabled = true;
        isGravityActive = true;
    }
}
