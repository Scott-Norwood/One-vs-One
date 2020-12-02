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
        #region Events
        // Create an event system that triggers when an enemy is spawned.
        public delegate void SpawnEnemyDelegate(WaveEnemy enemy);
        public static event SpawnEnemyDelegate OnSpawnEnemy;

        // Create an event system that triggers when a wave starts spawning.
        public delegate void SpawnWaveDelegate(Wave wave);
        public static event SpawnWaveDelegate OnSpawnWave;

        // Create an event system that triggers when a wave finishes spawning.
        public delegate void SpawnedWaveDelegate(Wave wave);
        public static event SpawnedWaveDelegate OnSpawnedWave;

        // Create an event system that triggers when all enemies are killed.
        public delegate void OnWaveKilledDelegate(Wave wave);
        public static event OnWaveKilledDelegate OnWaveKilled;

        // Create an event system that triggers when a step starts spawning.
        public delegate void SpawnStepDelegate(Step step);
        public static event SpawnStepDelegate OnSpawnStep;

        // Create an event system that triggers when a step finishes spawning.
        public delegate void SpawnedStepDelegate(Step step);
        public static event SpawnedStepDelegate OnSpawnedStep;
        #endregion

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
        private readonly List<KeyValuePair<Wave, WaveEnemy>> enemyReferences = new List<KeyValuePair<Wave, WaveEnemy>>();
        #endregion

        #region Awake() Method
        private void Awake()
        {
            // This is the singleton pattern. We want to make sure that only one WaveManager exists!
            if (_instance != null && _instance != this)
            {
                // If is already a WaveManager instance, delete this one.
                Destroy(this);
            }
            else
            {
                // If there is no WaveManager instance already present, then create a reference to this one.
                _instance = this;
            }
        }
        #endregion

        #region Start() Method
        private void Start()
        {
            // Make sure that you have references to the prefabs, and if you don't, throw an error to let you know you're being silly.
            if(prefabs.Count != Enum.GetNames(typeof(EnemyType)).Length)
            {
                Debug.LogWarning("Prefabs dictionary entry count does not match up with EnemyTypes entry count! Did you forget to add a reference to a prefab, or did you duplicate an entry?");
            }
        }
        #endregion

        #region SpawnWave() Enumerator
        /// <summary>
        /// The more advanced version of SpawnWave(). 
        /// Only resort to this function unless you have knowledge about how IEnumerators work.
        /// </summary>
        /// <param name="waveNumber">The wave to spawn.</param>
        /// <returns>Returns IEnumerator, letting you yield until all the enemies in the wave are killed.</returns>
        public IEnumerator SpawnWave(int waveNumber)
        {
            // Check to see if the wave exists. If it doesn't, then don't try and spawn the wave only to fail miserably.
            if (waves.Count < waveNumber)
            {
                Debug.LogError("Wave does not exist!");
            }

            // Start the Coroutine that will process the wave.
            yield return StartCoroutine(ProcessWave(waves[waveNumber]));

            // Call the proper event for beating a wave.
            OnWaveKilled?.Invoke(waves[waveNumber]);
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
            // Invoke the proper event for spawning a wave.
            OnSpawnWave?.Invoke(wave);

            // Loop through all of the steps.
            for (int i = 0; i < wave.steps.Count; i++)
            {
                // If the step requires us to wait until the enemies are killed, then wait! We also wait for last enemy to be killed in a wave before finishing the process.
                if (wave.steps[i].timingData.waitUntilKill && i != wave.steps.Count - 1)
                {
                    // Yield return this Coroutine so that the execution of ProcessWave does not finish until the current step finishes execution.
                    yield return StartCoroutine(ProcessStep(wave.steps[i], wave, false));
                } 
                else if (i == wave.steps.Count - 1) 
                {
                    // Yield return this Coroutine so that the execution of ProcessWave does not finish until the current step finishes execution.
                    yield return StartCoroutine(ProcessStep(wave.steps[i], wave, true));
                } 
                else
                {
                    // We do not want to wait for the step to process before moving onto the next one.
                    StartCoroutine(ProcessStep(wave.steps[i], wave, false));
                }

                // Wait the time specified in the current step data before moving onto the next step, unless it is the last step.
                if(i != wave.steps.Count - 1)
                {
                    yield return new WaitForSeconds(wave.steps[i].timingData.timeUntilNextStep);
                }
            }
        }
        #endregion

        #region ProcessStep() Enumerator()
        /// <summary>
        /// Processes a step, spawning enemies according to step data.
        /// </summary>
        /// <param name="step">The step to process.</param>
        /// <param name="lastStep">Is this the last step in the wave? Used for event system only.</param>
        /// <returns>Returns an IEnumerator for yielding until the step is done processing.</returns>
        private IEnumerator ProcessStep(Step step, Wave wave, bool lastStep)
        {
            // We want to let the event system know that we started to spawn a step.
            OnSpawnStep?.Invoke(step);

            // Randomize the enemy count.
            int enemies = Random.Range(step.enemyData.minNumberOfEnemies, step.enemyData.maxNumberOfEnemies);

            // Create a list for storing all of the references to enemies spawned in the current step.
            List<WaveEnemy> references = new List<WaveEnemy>();

            // If we have no chance of skipping this step, then don't divide by zero!
            if(step.skipChance != 0)
            {
                if(Random.value < step.skipChance / 100)
                {
                    // Break out of this Coroutine and stop execution early.
                    // If this is the last step in the wave, we want to notify the event system.
                    if (lastStep)
                    {
                        OnSpawnedWave?.Invoke(wave);
                    }

                    // Regardless, we want to notify the event system that we finished processing a step.
                    OnSpawnedStep?.Invoke(step);
                    yield break;
                }
            }

            // Loop through once for each enemy that we need to spawn.
            for (int i = 0; i < enemies; i++)
            {
                // Only wait for spacing time after the first iteration.
                if(i != 0)
                {
                    yield return new WaitForSeconds(Random.Range(step.timingData.minSpacing, step.timingData.maxSpacing));
                }

                // Spawn the enemy and add the references to the global (all waves) and local (current step) reference lists.
                WaveEnemy enemy = SpawnEnemy(step.enemyData.enemy);
                references.Add(enemy);

                KeyValuePair<Wave, WaveEnemy> pair = new KeyValuePair<Wave, WaveEnemy>(enemy.wave, enemy);
                enemyReferences.Add(pair);
            }

            // If this is the last step in the wave, we want to notify the event system.
            if (lastStep)
            {   
                OnSpawnedWave?.Invoke(wave);
            }

            // Regardless, we want to notify the event system that we finished processing a step.
            OnSpawnedStep?.Invoke(step);

            // While there are still references in the local (current step) reference list, yield.
            while(references.Count > 0)
            {
                // Loop through all of the references.
                for (int i = 0; i < references.Count; i++)
                {
                    // If it's null, then the object was deleted. We also want to check if the object is disabled for object pooling.
                    if(references[i].obj == null || references[i].obj.activeSelf == false)
                    {
                        // Remove the reference to the deleted object.
                        KeyValuePair<Wave, WaveEnemy> pair = new KeyValuePair<Wave, WaveEnemy>(references[i].wave, references[i]);
                        enemyReferences.Remove(pair);
                        references.RemoveAt(i);
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
        private WaveEnemy SpawnEnemy(EnemyType type)
        {
            // Spawn the enemy and save a reference to it for later manipulation by functions subscribed to the event.
            WaveEnemy enemy = new WaveEnemy
            {
                obj = GameObject.Instantiate(prefabs[(int)type])
            };

            // Invoke the proper event for spawning an enemy.
            OnSpawnEnemy?.Invoke(enemy);

            // Return the reference for later use.
            return enemy;
        }
        #endregion
    }
    #endregion

    #region WaveEnemy Class
    /// <summary>
    /// Holds data related to the enemy inside of the wave.
    /// </summary>
    [System.Serializable]
    public class WaveEnemy
    {
        // The type of this enemy.
        public EnemyType type;

        // The wave that this enemy spawned in.
        public Wave wave;

        // This is a reference to the enemy's GameObject.
        public GameObject obj;

        // When comparing a WaveEnemy we really just want to compare the wave that it's in.
        public override int GetHashCode()
        {
            return wave.GetHashCode();
        }
    }
    #endregion

    #region EnemyType Enum
    /// <summary>
    /// EnemyType stores a list of all the different enemy types.
    /// </summary>
    [System.Serializable]
    public enum EnemyType
    {
        Walker = 0,
        Crawler = 1,
        Seeker = 2,
        Tank = 3
    }
    #endregion
}
