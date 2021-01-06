using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Health : System_Health_Core
{
    public ParticleSystem rejuvEffect;
    public int sceneToChangeTo;
#pragma warning disable
    bool freeHeal = false;
#pragma warning enable

    //Empty class for the enemy that inherits all of healthsystem, modify it
    void Update()
    {
        if (GetHealth() == 0)
        {
            SceneManager.LoadSceneAsync(sceneToChangeTo, LoadSceneMode.Single);
        }
    }
    public override void DecreaseHealth(float damageAmount)
    {
        base.DecreaseHealth(damageAmount);
    }

    public override float GetHealth()
    {
        return base.GetHealth();
    }
    void OnCollisionEnter(Collision other)
    {
        Enemy_Health enemy_Health = other.gameObject.GetComponent<Enemy_Health>();

        if (other.collider.tag == "Enemy_Base" && enemy_Health.GetHealth() > 0)
        {
            DecreaseHealth(other.gameObject.GetComponent<Enemy_AI>().attackDamage);
        }
    }
}
