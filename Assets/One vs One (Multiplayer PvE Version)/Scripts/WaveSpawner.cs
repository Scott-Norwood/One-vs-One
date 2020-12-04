// Copyright (c) 2020 xtilly5000
// Licensed under the Creative Commons Attribution-ShareAlike 4.0 International license.
// Share — copy and redistribute the material in any medium or format.
// Adapt — remix, transform, and build upon the material for any purpose, even commercially.

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

namespace xtilly5000.Prototypes.WaveManager
{
    #region WaveSpawner Class
    /// <summary>
    /// WaveSpawner class is used for testing the WaveManager.
    /// It can be modified to serve a wide variety of purposes.
    /// </summary>
    public class WaveSpawner : MonoBehaviour
    {
        #region Variables
        // Spawn the enemies with the proper location and rotation.
        [Header("Spawn Settings")]
        public GameObject spawnPoint;

        public int currentWave
        {
            get { return _currentWave + 1; }
        }

        private int _currentWave;
        public int currentAttemptedWave;

        [Header("Wave Control Flow")]
        [Space]
        // The amount of time between wave spawns in seconds.
        public float timeBetweenWaves = 0f;

        // Do we want to pause the wave timer?
        public bool pause = false;

        // The amount of time left before the next wave starts in seconds.
        // Other classes might want to check this, for displaying on the screen etc.
        // We want those classes to be able to read this.
        [HideInInspector]
        public float TimeLeft { get; private set; }

        // Lets us know if the wave we spawned was killed or not.
        private bool waveKilled = false;

        #endregion

        #region Start() Method
        private void Start()
        {
            _currentWave = currentAttemptedWave;

            // Register all of the event functions for later use.
            WaveManager.OnWaveFinishedSpawning += OnWaveFinishedSpawning;
            WaveManager.OnWaveStartSpawning += OnWaveStartSpawning;
            WaveManager.OnWaveKilled += OnWaveKilled;

        }
        #endregion

        public void StartWaveTrigger()
        {
            StartCoroutine(WaveManager.Instance.SpawnWave(_currentWave));
        }

        #region Update() Method
        private void Update()
        {
            // Constantly count down the time left until the next wave, unless the countdown timer is paused.
            if (TimeLeft > 0f && pause == false && waveKilled == true)
            {
                TimeLeft -= 1 * Time.deltaTime;
            }
            else if (pause == false && waveKilled == true)
            {
                // Time ran out, so we want to spawn the next wave.
                TimeLeft = 0f;
                waveKilled = false;
                StartCoroutine(WaveManager.Instance.SpawnWave(_currentWave));
            }
        }
        #endregion

        #region OnSpawnedWave() Method
        private void OnWaveFinishedSpawning(Wave wave)
        {
            // Triggers when a wave finishes spawning.
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
            _currentWave++;
            currentAttemptedWave = _currentWave;
            waveKilled = true;
            TimeLeft = timeBetweenWaves;
        }
        #endregion
    }
    #endregion
}
