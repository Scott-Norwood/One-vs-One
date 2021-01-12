using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class EnemyIndexUnlocked : MonoBehaviour
{
    public GameObject[] enemiesToUnlock;
    public Transform enemyUnlockSpawn;

    public void ShowUnlockedEnemy(int enemyindex)
    {
        HideAllButActiveEnemyUnlock();
        if (PlayerPrefs.GetString(enemiesToUnlock[enemyindex].name) == "IsUnlocked")
        {
            enemiesToUnlock[enemyindex].SetActive(true);
        }
    }

    public void HideAllButActiveEnemyUnlock()
    {
        for (int i = 0; i < enemiesToUnlock.Length; i++)
        {
            enemiesToUnlock[i].SetActive(false);
        }
    }
}
