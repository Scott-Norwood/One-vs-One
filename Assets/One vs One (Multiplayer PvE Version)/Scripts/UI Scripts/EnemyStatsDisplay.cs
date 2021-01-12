using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyStatsDisplay : MonoBehaviour
{
    Enemy_Health enemy_Health;
    Enemy_AI enemy_AI;
    Enemy_Currency enemy_Currency;
    public TMP_Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        enemy_Health = GetComponent<Enemy_Health>();
        enemy_AI = GetComponent<Enemy_AI>();
        enemy_Currency = GetComponent<Enemy_Currency>();

        healthText.SetText("<b>Enemy Stats</b>" + "\n"
                            + "-Enemy Health: " + enemy_Health.healthMax + "\n"
                            + "-Enemy Damage: " + enemy_AI.attackDamage + "\n"
                            + "-Enemy Speed: " + enemy_AI.movementSpeedMin + " - " + enemy_AI.movementSpeedMax);
    }
}
