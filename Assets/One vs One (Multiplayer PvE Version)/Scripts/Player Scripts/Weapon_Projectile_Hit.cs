using UnityEngine;

public class Weapon_Projectile_Hit : MonoBehaviour
{
    public Weapon_Projectile_SO weapon_Projectile_SO;
    void OnCollisionEnter(Collision other)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject);
    }
}
