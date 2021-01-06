using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_FireDamage : MonoBehaviour
{
    public float fireTickRate;
    public int fireDamage;
    float fireDamagePerSecond;
    Enemy_Health enemy_Health;
    ParticleSystem fireSystem;

    void Start()
    {
        fireDamagePerSecond = 1f / (fireDamage / 60f);
    }
    void OnParticleCollision(GameObject other)
    {
        enemy_Health = GetComponent<Enemy_Health>();
        StartCoroutine(TakeFireDamage());
    }

    IEnumerator TakeFireDamage()
    {
        while (gameObject.activeSelf)
        {
            enemy_Health.DecreaseHealth((int)fireDamagePerSecond);
        }
        yield return new WaitForSeconds(fireTickRate);
    }
}
