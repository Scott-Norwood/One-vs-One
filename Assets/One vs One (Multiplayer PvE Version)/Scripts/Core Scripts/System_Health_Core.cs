using System;
using UnityEngine;
public class System_Health_Core : MonoBehaviour
{
    public int healthMax;
    [SerializeField] [ReadOnly] int health;

    public void Start()
    {
        health = healthMax;
    }

    public virtual int GetHealth() // Gets the current health
    {
        return health;
    }

    public virtual void DecreaseHealth(int damageAmount) // Takes damage amount and subtracts it from health until it hits health value 0;
    {
        health -= damageAmount;
        if (health < 0) health = 0;
    }

    public virtual void IncreaseHealth(int healAmount) // Takes heal amount and adds it to health until it hits healthmax value;
    {
        health += healAmount;
        if (health > healthMax) health = healthMax;
    }

    public virtual float GetHealthPercentage() // For Healthbar uses
    {
        return (float)health / healthMax;
    }
    public virtual void SetHealth(int setHealthValue)
    {
        healthMax = setHealthValue;
    }

}
