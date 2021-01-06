using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    //! Hard coded values in the damageovertime script, fix later

    Enemy_Health enemy_Health;
    void Start()
    {
        enemy_Health = GetComponent<Enemy_Health>();
        InvokeRepeating("DoT", 0, 5);
    }

    void DoT()
    {
        if (enemy_Health.GetHealth() > 0)
        {
            enemy_Health.DecreaseHealth(5);
            return;
        }
    }


    //* Implement some way to do damage over time for X seconds instead of forever
}
