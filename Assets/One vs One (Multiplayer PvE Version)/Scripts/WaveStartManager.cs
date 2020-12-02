using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xtilly5000.Prototypes.WaveManager;

public class WaveStartManager : MonoBehaviour
{
    int waveCount;
    void Start()
    {
        StartCoroutine(WaveManager.Instance.SpawnWave(0));

        WaveManager.OnWaveKilled += _OnWaveKilled;
        WaveManager.OnSpawnedStep += _OnSpawnedStep;
        WaveManager.OnSpawnedWave += _OnSpawnedWave;
        WaveManager.OnSpawnEnemy += _OnSpawnEnemy;
        WaveManager.OnSpawnStep += _OnSpawnStep;
        WaveManager.OnSpawnWave += _OnSpawnWave;
    }

    void _OnWaveKilled(Wave wave){
        Debug.Log("OnWaveKilled.");
    }

    void _OnSpawnedStep(Step step){
        Debug.Log("OnSpawnedStep.");
    }
    void _OnSpawnedWave(Wave wave){
        Debug.Log("OnSpawnedWave.");
    }
    void _OnSpawnEnemy(WaveEnemy waveEnemy){
        Debug.Log("OnSpawnEnemy.");
    }   
    void _OnSpawnStep(Step step){
        Debug.Log("OnSpawnStep.");
    } 
    void _OnSpawnWave(Wave wave){
        Debug.Log("OnSpawnWave.");
    }
}
