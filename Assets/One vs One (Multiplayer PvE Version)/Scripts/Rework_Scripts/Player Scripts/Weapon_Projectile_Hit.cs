using UnityEngine;

public class Weapon_Projectile_Hit : MonoBehaviour
{
    public int impactForce;
    HealthSystem healthSystem;
    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<Enemy_Health>())
        {

            healthSystem.DecreaseHealth(10);
            other.rigidbody.AddForce(transform.position, ForceMode.Impulse);
        }

        Lean.Pool.LeanPool.Despawn(gameObject);
    }
}
