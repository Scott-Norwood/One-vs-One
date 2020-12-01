// Copyright (c) 2020 xtilly5000
// Licensed under the Creative Commons Attribution-ShareAlike 4.0 International license.
// Share — copy and redistribute the material in any medium or format.
// Adapt — remix, transform, and build upon the material for any purpose, even commercially.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace xtilly5000.Prototypes.WaveManager
{
    #region WaveManager Class
    /// <summary>
    /// WaveManager manages the spawning of enemies in the scene.
    /// This class does NOT manage the contents of the waves, nor does it manage the enemies themselves.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        #region Variables
        // The wave data.
        public List<Wave> waves = new List<Wave>();

        // The list of references to prefabs for spawning enemies.
        // public Dictionary<EnemyTypes, GameObject> prefabs = new Dictionary<EnemyTypes, GameObject>();
        public List<GameObject> prefabs = new List<GameObject>();

        // Part of the Singleton Pattern. This is to make sure that only one instance of WaveManager exists at one time.
        public static WaveManager Instance { get { return _instance; } }
        private static WaveManager _instance;

        // Do not modify! This is the list of currently spawned enemies.
        private List<GameObject> enemyReferences = new List<GameObject>();
        #endregion

        #region Awake() Method
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
        }
        #endregion

        #region Start() Method
        private void Start()
        {
            // Make sure that you have references to the prefabs, and if you don't, throw an error to let you know you're being silly.
            if (prefabs.Count != Enum.GetNames(typeof(EnemyTypes)).Length)
            {
                Debug.LogWarning("Prefabs dictionary entry count does not match up with EnemyTypes entry count! Did you forget to add a reference to a prefab, or did you duplicate an entry?");
            }
        }
        #endregion

        #region SpawnWave() Method
        /// <summary>
        /// Spawns a wave in the scene.
        /// </summary>
        /// <param name="waveNumber">The wave to spawn.</param>
        public void SpawnWave(int waveNumber)
        {
            // Check to see if the wave exists. If it doesn't, then don't try and spawn the wave only to fail miserably.
            if (waves.Count < waveNumber)
            {
                Debug.LogError("Wave does not exist!");
            }

            StartCoroutine(ProcessWave(waves[waveNumber]));
        }
        #endregion

        #region SpawnWaveEnumerator() Enumerator
        /// <summary>
        /// The more advanced version of SpawnWave(). 
        /// Only resort to this function unless you have knowledge about how IEnumerators work.
        /// </summary>
        /// <param name="waveNumber">The wave to spawn.</param>
        /// <returns>Returns IEnumerator, letting you yield until all the enemies in the wave are killed.</returns>
        public IEnumerator SpawnWaveEnumerator(int waveNumber)
        {
            // Check to see if the wave exists. If it doesn't, then don't try and spawn the wave only to fail miserably.
            if (waves.Count < waveNumber)
            {
                Debug.LogError("Wave does not exist!");
            }

            // Start the Coroutine that will process the wave.
            StartCoroutine(ProcessWave(waves[waveNumber]));

            // Do not finish execution of this Coroutine until there are no enemies left.
            while (enemyReferences.Count > 0)
            {
                yield return null;
            }
        }
        #endregion

        #region ProcessWave() Enumerator
        /// <summary>
        /// Processes a wave and it's contents.
        /// This will loop through every step in a wave and execute it accordingly.
        /// </summary>
        /// <param name="wave">The wave to process.</param>
        /// <returns>Returns an IEnumerator for yielding until the wave is done processing.</returns>
        private IEnumerator ProcessWave(Wave wave)
        {
            // Loop through all of the steps.
            for (int i = 0; i < wave.steps.Count; i++)
            {
                // If the step requires us to wait until the enemies are killed, then wait!
                if (wave.steps[i].timingData.waitUntilKill)
                {
                    // Yield return this Coroutine so that the execution of ProcessWave does not finish until the current step finishes execution.
                    yield return StartCoroutine(ProcessStep(wave.steps[i]));
                }
                else
                {
                    StartCoroutine(ProcessStep(wave.steps[i]));
                }

                // Wait the time specified in the current step data before moving onto the next step.
                yield return new WaitForSeconds(wave.steps[i].timingData.timeUntilNextStep);
            }
        }
        #endregion

        #region ProcessStep() Enumerator()
        /// <summary>
        /// Processes a step, spawning enemies according to step data.
        /// </summary>
        /// <param name="step">The step to process.</param>
        /// <returns>Returns an IEnumerator for yielding until the step is done processing.</returns>
        private IEnumerator ProcessStep(Step step)
        {
            // Randomize the enemy count.
            int enemies = Random.Range(step.enemyData.minNumberOfEnemies, step.enemyData.maxNumberOfEnemies);

            // Create a list for storing all of the references to enemies spawned in the current step.
            List<GameObject> references = new List<GameObject>();

            // If we have no chance of skipping this step, then don't divide by zero!
            if (step.skipChance != 0)
            {
                if (Random.value < step.skipChance / 100)
                {
                    // Break out of this Coroutine and stop execution early.
                    yield break;
                }
            }

            // Loop through once for each enemy that we need to spawn.
            for (int i = 0; i < enemies; i++)
            {
                // Only wait for spacing time after the first iteration.
                if (i != 0)
                {
                    yield return new WaitForSeconds(step.timingData.spacing);
                }

                // Spawn the enemy and add the references to the global (all waves) and local (current step) reference lists.
                GameObject enemy = SpawnEnemy(step.enemyData.enemy);
                references.Add(enemy);
                enemyReferences.Add(enemy);
            }

            // While there are still references in the local (current step) reference list, yield.
            while (references.Count > 0)
            {
                // Loop through all of the references.
                for (int i = 0; i < references.Count; i++)
                {
                    // If it's null, then the object was deleted.
                    if (references[i] == null)
                    {
                        // Remove the reference to the deleted object.
                        references.RemoveAt(i);
                        enemyReferences.RemoveAt(i);
                    }
                }

                // Continue until there are no references left!
                yield return null;
            }
        }
        #endregion

        #region SpawnEnemy() Method
        /// <summary>
        /// This function handles the spawning of an enemy.
        /// </summary>
        /// <param name="type">The type of enemy to spawn.</param>
        /// <returns>Returns a reference to the spawned GameObject.</returns>
        private GameObject SpawnEnemy(EnemyTypes type)
        {
            //Spawns enemies wherever the gameobject this script is attached to is.
            GameObject enemy = GameObject.Instantiate(prefabs[(int)type], transform.position, Quaternion.identity);
            return enemy;
        }
        #endregion
    }
    #endregion

    #region EnemyTypes Enum
    [System.Serializable]
    public enum EnemyTypes
    {
        Walker = 0,
        //Crawler = 1,
        //Seeker = 2,
        //Tank = 3
    }
    #endregion
}
