// Copyright (c) 2020 xtilly5000
// Licensed under the Creative Commons Attribution-ShareAlike 4.0 International license.
// Share — copy and redistribute the material in any medium or format.
// Adapt — remix, transform, and build upon the material for any purpose, even commercially.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace xtilly5000.Prototypes.WaveManager
{
    #region WaveManager Class
    /// <summary>
    /// WaveManager manages the spawning of enemies in the scene.
    /// This class does NOT contain data related to the contents of waves, nor does it manage the enemies themselves.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        #region Events
        // Create an event system that triggers when an enemy is spawned.
        public delegate void SpawnEnemyDelegate(WaveEnemy enemy);
        public static event SpawnEnemyDelegate OnSpawnEnemy;

        // Create an event system that triggers when a wave starts spawning.
        public delegate void WaveStartSpawningDelegate(Wave wave);
        public static event WaveStartSpawningDelegate OnWaveStartSpawning;

        // Create an event system that triggers when a wave finishes spawning.
        public delegate void WaveFinishedSpawningDelegate(Wave wave);
        public static event WaveFinishedSpawningDelegate OnWaveFinishedSpawning;

        // Create an event system that triggers when all enemies in a wave are killed.
        public delegate void OnWaveKilledDelegate(Wave wave);
        public static event OnWaveKilledDelegate OnWaveKilled;

        // Create an event system that triggers when a step starts spawning.
        private delegate void StepStartSpawningDelegate(Step step);
        private static event StepStartSpawningDelegate OnStepStartSpawning;

        // Create an event system that triggers when a step finishes spawning.
        private delegate void StepFinishedSpawningDelegate(Step step);
        private static event StepFinishedSpawningDelegate OnStepFinishedSpawning;
        #endregion

        #region Variables
        // The wave data.
        [Header("List of Waves")]
        public List<Wave> waves = new List<Wave>();

        // The list of references to prefabs for spawning enemies.
        [Header("Prefab References")]
        [Space]
        public List<GameObject> prefabs = new List<GameObject>();

        // Part of the Singleton Pattern. This is to make sure that only one instance of WaveManager exists at one time.
        public static WaveManager Instance { get { return _instance; } }
        private static WaveManager _instance;

        // Do not modify! This is the list of currently spawned enemies.
        private readonly List<KeyValuePair<Wave, WaveEnemy>> enemyReferences = new List<KeyValuePair<Wave, WaveEnemy>>();
        #endregion

        #region Awake() Method
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "This function is being used by Unity itself, so we do not need to call it locally.")]
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

        #region SpawnWave() Enumerator
        /// <summary>
        /// Spawns a wave using a direct reference to the Wave.
        /// </summary>
        /// <param name="wave">The wave to spawn.</param>
        /// <returns>Returns IEnumerator, letting you yield until all the enemies in the wave are killed.</returns>
        public IEnumerator SpawnWave(Wave wave)
        {
            // Start the Coroutine that will process the wave.
            yield return StartCoroutine(ProcessWave(wave));

            // Call the proper event for beating a wave.
            OnWaveKilled?.Invoke(wave);
        }
        #endregion

        #region SpawnWave() Enumerator
        /// <summary>
        /// Spawns a wave using a reference to a Wave in the waves list.
        /// </summary>
        /// <param name="waveNumber">The wave to spawn.</param>
        /// <returns>Returns IEnumerator, letting you yield until all the enemies in the wave are killed.</returns>
        public IEnumerator SpawnWave(int waveNumber)
        {
            // Check to see if the wave exists. If it doesn't, then don't try and spawn the wave only to fail miserably.
            if (waves.Count < waveNumber + 1f)
            {
                Debug.LogWarning("Wave does not exist!");
                yield break;
            }
            else
            {
                // Start the Coroutine that will process the wave.
                yield return StartCoroutine(ProcessWave(waves[waveNumber]));

                // Call the proper event for beating a wave.
                OnWaveKilled?.Invoke(waves[waveNumber]);
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
            // Invoke the proper event for spawning a wave.
            OnWaveStartSpawning?.Invoke(wave);

            // Loop through all of the steps.
            for (int i = 0; i < wave.steps.Count; i++)
            {
                // If the step requires us to wait until the enemies are killed, then wait! We also wait for last enemy to be killed in a wave before finishing the process.
                if (wave.steps[i].waitUntilKill && i != wave.steps.Count - 1)
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
                if (i != wave.steps.Count - 1)
                {
                    // Randomize the time between steps.
                    float waitTime = Random.Range(wave.steps[i].minTimeUntilNextStep, wave.steps[i].maxTimeUntilNextStep);
                    yield return new WaitForSeconds(waitTime);
                }
            }
        }
        #endregion

        #region ProcessStep() Enumerator
        /// <summary>
        /// Processes a step, spawning enemies according to step data.
        /// </summary>
        /// <param name="step">The step to process.</param>
        /// <param name="lastStep">Is this the last step in the wave? Used for event system only.</param>
        /// <returns>Returns an IEnumerator for yielding until the step is done processing.</returns>
        private IEnumerator ProcessStep(Step step, Wave wave, bool lastStep)
        {
            // We want to let the event system know that we started to spawn a step.
            OnStepStartSpawning?.Invoke(step);

            // Randomize the enemy count.
            int enemies = Random.Range(step.minNumberOfEnemies, step.maxNumberOfEnemies);

            // Create a list for storing all of the references to enemies spawned in the current step.
            List<WaveEnemy> references = new List<WaveEnemy>();

            // If we have no chance of skipping this step, then don't divide by zero!
            if (step.skipChance != 0)
            {
                if (Random.value < step.skipChance / 100)
                {
                    // Break out of this Coroutine and stop execution early.
                    // If this is the last step in the wave, we want to notify the event system.
                    if (lastStep)
                    {
                        OnWaveFinishedSpawning?.Invoke(wave);
                    }

                    // Regardless, we want to notify the event system that we finished processing a step.
                    OnStepFinishedSpawning?.Invoke(step);
                    yield break;
                }
            }

            // Loop through once for each enemy that we need to spawn.
            for (int i = 0; i < enemies; i++)
            {
                // Only wait for spacing time after the first iteration.
                if (i != 0)
                {
                    yield return new WaitForSeconds(Random.Range(step.minSpacing, step.maxSpacing));
                }

                if (prefabs.Count <= step.id)
                {
                    Debug.LogWarning("Could not spawn enemy. The enemy id has no prefab associated with it!");
                }
                else
                {
                    // Spawn the enemy and add the references to the global (all waves) and local (current step) reference lists.
                    WaveEnemy enemy = SpawnEnemy(step.id);
                    references.Add(enemy);

                    KeyValuePair<Wave, WaveEnemy> pair = new KeyValuePair<Wave, WaveEnemy>(enemy.wave, enemy);
                    enemyReferences.Add(pair);
                }
            }

            // If this is the last step in the wave, we want to notify the event system.
            if (lastStep)
            {
                OnWaveFinishedSpawning?.Invoke(wave);
            }

            // Regardless, we want to notify the event system that we finished processing a step.
            OnStepFinishedSpawning?.Invoke(step);

            // While there are still references in the local (current step) reference list, yield.
            while (references.Count > 0)
            {
                // Loop through all of the references.
                for (int i = 0; i < references.Count; i++)
                {
                    // If it's null, then the object was deleted. We also want to check if the object is disabled for object pooling.
                    if (references[i].obj == null || references[i].obj.activeSelf == false)
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
        private WaveEnemy SpawnEnemy(int id)
        {
            // Spawn the enemy and save a reference to it for later manipulation by functions subscribed to the event.
            WaveEnemy enemy = new WaveEnemy
            {
                obj = GameObject.Instantiate(prefabs[id])
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
        // The id of the enemy. This is NOT a unique value!
        public int id;

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

    #region Step Class
    /// <summary>
    /// This class provides the basic data structure for creating steps.
    /// </summary>
    [System.Serializable]
    public class Step
    {
        #region Variables
        // The maximum amount of seconds before the next step.
        [Tooltip("The maximum amount of seconds before the next step.")]
        public float maxTimeUntilNextStep;

        // The minimum amount of seconds before the next step.
        [Tooltip("The minimum amount of seconds before the next step.")]
        public float minTimeUntilNextStep;

        // The minimum number of time between spawns in seconds.
        [Tooltip("The minimum number of time between spawns in seconds.")]
        public float minSpacing;

        // The maximum number of time between spawns in seconds.
        [Tooltip("The maximum number of time between spawns in seconds.")]
        public float maxSpacing;

        // Do we want to wait until the enemies in this step are killed before moving on to the next step?
        [Tooltip("Do we want to wait until the enemies in this step are killed before moving on to the next step?")]
        public bool waitUntilKill;

        // The ID of the enemy that we want to spawn.
        [Tooltip("The ID of the enemy that we want to spawn.")]
        public int id;

        // The maximum number of enemies to spawn this step.
        [Tooltip("The maximum number of enemies to spawn this step.")]
        public int maxNumberOfEnemies;

        // The minimum number of enemies to spawn this step.
        [Tooltip("The minimum number of enemies to spawn this step.")]
        public int minNumberOfEnemies;

        // The chance, in a percentage, to skip this step.
        [Tooltip("The chance, in a percentage, to skip this step.")]
        public int skipChance;

        // Should the editor show advanced customization options?
        [Tooltip("Should the editor show advanced customization options?")]
        public bool isAdvanced;
        #endregion
    }
    #endregion
}
