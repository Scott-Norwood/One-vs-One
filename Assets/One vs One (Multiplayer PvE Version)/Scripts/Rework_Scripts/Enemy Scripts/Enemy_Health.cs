using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    HealthSystem healthSystem;
    void Start()
    {
        healthSystem = new HealthSystem(100);
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        Debug.Log(healthSystem.GetHealthPercentage());
    }
}
