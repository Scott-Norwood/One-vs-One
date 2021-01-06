using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCurrency_Bodyshot : MonoBehaviour
{
    public int bodyshotValue;
    Enemy_Currency enemy_Currency;
    Enemy_Health enemy_Health;

    void Start()
    {
        enemy_Currency = GetComponentInParent<Enemy_Currency>();
        enemy_Health = GetComponentInParent<Enemy_Health>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            enemy_Currency.BodyShot(bodyshotValue);
            enemy_Health.DecreaseHealth((int)other.gameObject.GetComponent<Weapon_Projectile_Hit>().weapon_Projectile_SO.projectileDamage);
            if (enemy_Health.GetHealth() == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
