using System.Collections;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public ParticleSystem particleEffect;
    public AudioSource audioSource;
    public int damage = 10;
    public int power = 10;
    public int radius = 5;
    public int upForce = 2;
    public float fuseTime;
    // Start is called before the first frame update

    void ExplosionBlast()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject != this.gameObject && rb.gameObject.tag != "Grenade")
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upForce, ForceMode.Impulse);
            }
        }
    }

    void ExplosionDamage()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in colliders)
        {
            Enemy_Health enemy_Health = hit.GetComponent<Enemy_Health>();
            if (enemy_Health != null)
            {
                enemy_Health.DecreaseHealth(damage);
            }
        }
    }

    bool hasHitSomething = false;
    void OnCollisionEnter(Collision other)
    {
        if (!hasHitSomething)
        {
            StartCoroutine(ExplosionTimer());
            hasHitSomething = true;
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
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
