using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : HealthSystem
{
    //Empty class for the enemy that inherits all of healthsystem, modify it

    void Update()
    {
        if (GetHealth() == 0)
        {
            print("Enemy Killed.");
            Destroy(gameObject);
        }
    }

    public override void DecreaseHealth(int damageAmount)
    {
        base.DecreaseHealth(damageAmount);
    }

    public override int GetHealth()
    {
        return base.GetHealth();
    }
}
