// Copyright (c) 2020 xtilly5000
// Licensed under the Creative Commons Attribution-ShareAlike 4.0 International license.
// Share — copy and redistribute the material in any medium or format.
// Adapt — remix, transform, and build upon the material for any purpose, even commercially.

using UnityEngine;

namespace xtilly5000.Prototypes.WaveManager
{
    #region WaveSpawnerTest Class
    /// <summary>
    /// WaveSpawnerTest class is used for testing the WaveManager.
    /// It can be modified to serve a wide variety of purposes.
    /// </summary>
    public class InfiniteSpawnTest : MonoBehaviour
    {
        #region Variables
        // Spawn the enemies with the proper location and rotation.
        [Header("Spawn Settings")]
        public GameObject spawnPoint;
        public int currentWave;

        #endregion

        #region Start() Method
        private void Start()
        {
            // Register all of the event functions for later use.
            WaveManager.OnSpawnEnemy += OnSpawnEnemy;
            WaveManager.OnWaveKilled += OnWaveKilled;

            // Start spawning the first wave!
            StartCoroutine(WaveManager.Instance.SpawnWave(currentWave));
        }
        #endregion

        #region Update() Method
        private void Update()
        {
            if (currentWave > WaveManager.Instance.waves.Count)
            {
                Wave wave = new Wave();
                wave.steps.Add(new Step()
                {
                    maxNumberOfEnemies = Random.Range(1, 2) * currentWave,
                    minNumberOfEnemies = 1
                });

                StartCoroutine(WaveManager.Instance.SpawnWave(wave));
            }
        }
        #endregion

        #region OnSpawnEnemy() Method
        private void OnSpawnEnemy(WaveEnemy enemy)
        {
            // Triggers when an enemy is spawned.
            // We want to set the proper position and rotation for the spawned enemy.
            enemy.obj.transform.position = spawnPoint.transform.position;
            enemy.obj.transform.rotation = spawnPoint.transform.rotation;
        }
        #endregion

        #region OnWaveKilled() Method
        private void OnWaveKilled(Wave wave)
        {
            // Triggers when all enemies in a wave are killed.
            currentWave++;
        }
        #endregion
    }
    #endregion
}
