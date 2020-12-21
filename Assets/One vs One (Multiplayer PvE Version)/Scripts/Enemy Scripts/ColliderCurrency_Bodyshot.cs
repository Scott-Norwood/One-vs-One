using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCurrency_Bodyshot : MonoBehaviour
{
    public int bodyshotValue;
    Enemy_Currency enemy_Currency;
    Enemy_Health enemy_Health;
    Weapon_Shoot weapon_Shoot;

    void Start()
    {
        enemy_Currency = GetComponentInParent<Enemy_Currency>();
        enemy_Health = GetComponentInParent<Enemy_Health>();
        weapon_Shoot = FindObjectOfType<Weapon_Shoot>();
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Projectile")
        {
            weapon_Shoot.roundsOnTarget++;
            enemy_Currency.BodyShot(bodyshotValue);
            enemy_Health.DecreaseHealth((int)other.gameObject.GetComponent<Weapon_Projectile_Hit>().weapon_Projectile_SO.projectileDamage);
            if (enemy_Health.GetHealth() == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
