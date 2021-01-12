using System.Collections;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public GrenadeSO grenade;
    public AudioSource audioSource;
    bool hasHitSomething = false;
    void OnCollisionEnter(Collision other)
    {
        if (other is null)
        {
            throw new System.ArgumentNullException(nameof(other));
        }

        if (!hasHitSomething)
        {
            if (grenade.grenadeType == GrenadeType.FragGrenade) StartCoroutine(ExplosionTimer());
            if (grenade.grenadeType == GrenadeType.ImplosionGrenade) StartCoroutine(WaitForRagdollPhysics());
            if (grenade.grenadeType == GrenadeType.AntigravityGrenade) StartCoroutine(WaitForRagdollPhysics());
            hasHitSomething = true;
        }
        if (grenade.grenadeType == GrenadeType.FireGrenade) //! Old method, but it works.
        {
            Instantiate(grenade.explosionEffect, gameObject.transform.position, Quaternion.identity);
            Instantiate(grenade.groundEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject, 3f);
        }
    }

    void BitchFragExplosionEffect()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, grenade.explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            RagdollController ragdoll = hit.GetComponentInChildren<RagdollController>();

            if (rb != null && rb.gameObject != this.gameObject && rb.gameObject.tag != "Grenade" && grenade.grenadeType == GrenadeType.FragGrenade)
            {
                rb.AddExplosionForce(grenade.grenadeForce, explosionPosition, grenade.explosionRadius, grenade.upwardExplosionForce, ForceMode.Impulse);
            }
        }
    }

    void BitchFragGrenade()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, grenade.explosionRadius);
        foreach (Collider hit in colliders)
        {
            Enemy_Health enemy_Health = hit.GetComponent<Enemy_Health>();

            if (enemy_Health != null && grenade.grenadeType == GrenadeType.FragGrenade)
            {
                enemy_Health.DecreaseHealth(grenade.grenadeDamage);
            }
        }
    }

    void ImplosionGrenade()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, grenade.explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            RagdollController ragdoll = hit.GetComponentInChildren<RagdollController>();

            if (rb != null && ragdoll != null && rb.gameObject != this.gameObject && rb.gameObject.tag != "Grenade" && grenade.grenadeType == GrenadeType.ImplosionGrenade)
            {
                ragdoll.DeactivateGravity();
                rb.useGravity = false;
            }
        }
    }

    void ImplosionGrenadeEffect()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, grenade.explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb.gameObject != this.gameObject && rb.gameObject.tag != "Grenade" && grenade.grenadeType == GrenadeType.ImplosionGrenade)
            {
                Vector3 offset = rb.transform.position - new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                rb.AddForce(-offset * grenade.impulseForce);
            }

            Enemy_Health enemy_Health = hit.GetComponent<Enemy_Health>();

            if (enemy_Health != null && grenade.grenadeType == GrenadeType.ImplosionGrenade)
            {
                enemy_Health.DecreaseHealth(grenade.grenadeDamage);
            }
        }
    }

    void AntiGravityGrenade()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, grenade.explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            RagdollController ragdoll = hit.GetComponentInChildren<RagdollController>();

            if (rb != null && ragdoll != null && rb.gameObject != this.gameObject && rb.gameObject.tag != "Grenade" && grenade.grenadeType == GrenadeType.AntigravityGrenade)
            {
                rb.useGravity = false;
                ragdoll.DeactivateGravity();
            }
        }
    }

    void AntiGravityGrenadeEffect()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, grenade.explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb.gameObject != this.gameObject && rb.gameObject.tag != "Grenade" && grenade.grenadeType == GrenadeType.AntigravityGrenade)
            {
                rb.AddForce(Vector3.up * grenade.impulseForce);
            }
            Enemy_Health enemy_Health = hit.GetComponent<Enemy_Health>();

            if (enemy_Health != null && grenade.grenadeType == GrenadeType.AntigravityGrenade)
            {
                enemy_Health.DecreaseHealth(grenade.grenadeDamage); //! Implement if you want damage on antigravity
            }
        }
    }

    IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(grenade.fuseTime); //fusetime
        if (grenade.grenadeType == GrenadeType.FragGrenade) BitchFragGrenade();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (grenade.grenadeType == GrenadeType.FragGrenade) Instantiate(grenade.explosionEffect, transform.position, Quaternion.Euler(0, 0, 0));
        audioSource.PlayOneShot(grenade.explosionSound);
        if (grenade.grenadeType == GrenadeType.FragGrenade) BitchFragExplosionEffect();
        Destroy(gameObject, 2f);
    }

    IEnumerator WaitForRagdollPhysics()
    {
        yield return new WaitForSeconds(grenade.fuseTime);
        if (grenade.grenadeType == GrenadeType.ImplosionGrenade) { ImplosionGrenade(); }
        if (grenade.grenadeType == GrenadeType.AntigravityGrenade) { AntiGravityGrenade(); }
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (grenade.grenadeType == GrenadeType.ImplosionGrenade) Instantiate(grenade.explosionEffect, transform.position, Quaternion.Euler(90, 0, 0));
        if (grenade.grenadeType == GrenadeType.AntigravityGrenade) Instantiate(grenade.explosionEffect, transform.position, Quaternion.Euler(-90, 0, 0));
        audioSource.PlayOneShot(grenade.explosionSound);
        if (grenade.grenadeType == GrenadeType.ImplosionGrenade) { InvokeRepeating("ImplosionGrenadeEffect", 0, grenade.implosionPulseTime); }
        if (grenade.grenadeType == GrenadeType.AntigravityGrenade) { AntiGravityGrenadeEffect(); }
        Destroy(gameObject, 3f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, grenade.explosionRadius);
    }
}
