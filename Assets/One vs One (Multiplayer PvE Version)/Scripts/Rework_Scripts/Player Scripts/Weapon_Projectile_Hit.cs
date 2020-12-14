using UnityEngine;

public class Weapon_Projectile_Hit : MonoBehaviour
{
    //public int impactForce;
    Enemy_Health enemy_Health;
    void OnCollisionEnter(Collision other)
    {
        enemy_Health = other.gameObject.GetComponent<Enemy_Health>();

        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<Enemy_Health>() != null)
        {
            enemy_Health.DecreaseHealth(5);
            other.rigidbody.AddForce(transform.position, ForceMode.Impulse);
            Debug.Log(other.gameObject.name);
        }

        Destroy(gameObject);
    }
}
