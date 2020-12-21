using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : System_Health_Core
{
    Enemy_Currency enemy_Currency;
    System_Currency_Core player_Currency;
    RagdollController ragdollController;
    Enemy_AI enemy;

    void Awake()
    {
        enemy_Currency = GetComponent<Enemy_Currency>();
        player_Currency = FindObjectOfType<System_Currency_Core>();
        ragdollController = GetComponentInChildren<RagdollController>();
        enemy = GetComponent<Enemy_AI>();

    }

    bool hasDied = false;
    //Empty class for the enemy that inherits all of healthsystem, modify it
    void Update()
    {
        if (!hasDied)
        {
            if (GetHealth() == 0)
            {
                print("Enemy Killed.");
                player_Currency.IncreasePoints(enemy_Currency.enemyValue);
                enemy.enabled = false;
                ragdollController.ActivateRagdoll();
                //Destroy(gameObject);
                StartCoroutine(WaitToDespawn());
                hasDied = true;
            }
        }
    }

    IEnumerator WaitToDespawn()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
