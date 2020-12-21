using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCurrency_Headshot : MonoBehaviour
{
    Enemy_Currency enemy_Currency;
    Enemy_Health enemy_Health;
    Weapon_Shoot weapon_Shoot;
    public int headshotValue;
    public int headshotDamageMultiplier = 1;
    public Collider bodyCollider;

    void Start()
    {
        enemy_Currency = GetComponentInParent<Enemy_Currency>();
        enemy_Health = GetComponentInParent<Enemy_Health>();
        weapon_Shoot = FindObjectOfType<Weapon_Shoot>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            weapon_Shoot.roundsOnTarget++;
            enemy_Currency.HeadShot(headshotValue);
            enemy_Health.DecreaseHealth((int)other.gameObject.GetComponent<Weapon_Projectile_Hit>().weapon_Projectile_SO.projectileDamage * headshotDamageMultiplier);
            if (enemy_Health.GetHealth() == 0)
            {
                Destroy(bodyCollider);
            }
        }
    }
}
