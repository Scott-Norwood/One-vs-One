using System.Collections;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public ParticleSystem particleEffect;
    public ParticleSystem fireGroundEffect;
    public AudioSource audioSource;
    public int grenadeDamage = 10;
    public int grenadeForce = 10;
    public int explosionRadius = 5;
    public int upwardExplosionForce = 2;
    public float fuseTime;
    public float impulseForce;
    public bool isBitchFragGrenade = false; //! Implement enum for this
    public bool isFireGrenade = false; //! Implement enum for this
    public bool isImplosionGrenade = false; //! Implement enum for this
    public bool isAntigravityGrenade = false; //! Implement enum for this

    void ExplosionBlast()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            //* Frag Grenade
            if (rb != null && rb.gameObject != this.gameObject && rb.gameObject.tag != "Grenade" && isBitchFragGrenade)
            {
                rb.AddExplosionForce(grenadeForce, explosionPosition, explosionRadius, upwardExplosionForce, ForceMode.Impulse);
            }

            //* Implosion Grenade
            if (rb != null && rb.gameObject != this.gameObject && rb.gameObject.tag != "Grenade" && isImplosionGrenade)
            {
                grenadeDamage = 0;
                rb.isKinematic = false;
                rb.useGravity = false;
                Vector3 offset = rb.transform.position - transform.position;
                rb.AddForce(-offset * impulseForce);
            }

            //* Antigravity Grenade
            if (rb != null && rb.gameObject != this.gameObject && rb.gameObject.tag != "Grenade" && isAntigravityGrenade)
            {
                grenadeDamage = 0;
                rb.isKinematic = false;
                rb.useGravity = false;
                rb.AddForce(Vector3.up * 500f);
            }
        }
    }

    void ExplosionDamage()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Enemy_Health enemy_Health = hit.GetComponent<Enemy_Health>();
            if (enemy_Health != null)
            {
                enemy_Health.DecreaseHealth(grenadeDamage);
            }
        }
    }

    bool hasHitSomething = false;
    void OnCollisionEnter(Collision other)
    {
        if (other is null)
        {
            throw new System.ArgumentNullException(nameof(other));
        }

        if (!hasHitSomething)
        {
            StartCoroutine(ExplosionTimer());
            hasHitSomething = true;
        }
        if (isFireGrenade) //! Old method, but it works.
        {
#pragma warning disable
            fireGroundEffect.playOnAwake = true;
#pragma warning enable
            Instantiate(fireGroundEffect, gameObject.transform.position, Quaternion.identity);
        }
    }

    IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(fuseTime); //fusetime
        ExplosionDamage();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        particleEffect.Play();
        audioSource.Play();
        ExplosionBlast();
        Destroy(gameObject, 2f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, explosionRadius);
    }
}
