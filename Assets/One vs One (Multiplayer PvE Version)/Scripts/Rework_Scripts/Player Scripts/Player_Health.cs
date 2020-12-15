using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : System_Health_Core
{
    //Empty class for the enemy that inherits all of healthsystem, modify it

    public override void DecreaseHealth(int damageAmount)
    {
        base.DecreaseHealth(damageAmount);
    }

    public override int GetHealth()
    {
        return base.GetHealth();
    }
}
