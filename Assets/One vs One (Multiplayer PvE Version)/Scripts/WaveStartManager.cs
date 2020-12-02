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
    public class WaveStartManager : MonoBehaviour
    {
        #region Variables
        // Spawn the enemies with the proper location and rotation.
        public GameObject spawnPoint;

        // Keeps track of the current wave.
        public int currentWave = 0;

        // Do we want to pause the wave timer?
        public bool pause = false;

        // The amount of time between wave spawns in seconds.
        public float timeBetweenWaves = 0f;

        // The amount of time left before the next wave starts in seconds.
        private float timeLeft = 0f;

        // Lets us know if the wave we spawned was killed or not.
        private bool waveKilled = false;
        #endregion

        #region Start() Method
        private void Start()
        {
            // Register all of the event functions for later use.
            WaveManager.OnStepFinishedSpawning += OnStepFinishedSpawning;
            WaveManager.OnWaveFinishedSpawning += OnWaveFinishedSpawning;
            WaveManager.OnSpawnEnemy += OnSpawnEnemy;
            WaveManager.OnStepStartSpawning += OnStepStartSpawning;
            WaveManager.OnWaveStartSpawning += OnWaveStartSpawning;
            WaveManager.OnWaveKilled += OnWaveKilled;

            // Start spawning the first wave!
            StartCoroutine(WaveManager.Instance.SpawnWave(currentWave));
        }
        #endregion

        #region Update() Method
        private void Update()
        {
            // Constantly count down the time left until the next wave, unless the countdown timer is paused.
            if(timeLeft > 0f && pause == false && waveKilled == true)
            {
                timeLeft -= 1 * Time.deltaTime;
            } else if(pause == false && waveKilled == true)
            {
                // Time ran out, so we want to spawn the next wave.
                timeLeft = 0f;
                waveKilled = false;
                StartCoroutine(WaveManager.Instance.SpawnWave(currentWave));
            }
        }
        #endregion

        #region OnSpawnedStep() Method
        private void OnStepFinishedSpawning(Step step)
        {
            // Triggers when a step finishes spawning.
        }
        #endregion

        #region OnSpawnedWave() Method
        private void OnWaveFinishedSpawning(Wave wave)
        {
            // Triggers when a wave finishes spawning.
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

        #region OnSpawnStep() Method
        private void OnStepStartSpawning(Step step)
        {
            // Triggers when a step starts spawning.
        }
        #endregion

        #region OnSpawnWave() Method
        private void OnWaveStartSpawning(Wave wave)
        {
            // Triggers when a wave starts spawning.
        }
        #endregion

        #region OnWaveKilled() Method
        private void OnWaveKilled(Wave wave)
        {
            // Triggers when all enemies in a wave are killed.
            currentWave++;
            waveKilled = true;
            timeLeft = timeBetweenWaves;
        }
        #endregion
    }
    #endregion
}
