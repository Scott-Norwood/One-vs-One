// Copyright (c) 2020 xtilly5000
// Licensed under the Creative Commons Attribution-ShareAlike 4.0 International license.
// Share — copy and redistribute the material in any medium or format.
// Adapt — remix, transform, and build upon the material for any purpose, even commercially.

using System.Collections.Generic;
using UnityEngine;

namespace xtilly5000.Prototypes.WaveManager
{
    #region Wave Class
    /// <summary>
    /// This class provides the basic data structure for creating waves.
    /// </summary>
    [CreateAssetMenu(fileName = "Wave", menuName = "Wave Manager/Wave")]
    public class Wave : ScriptableObject
    {
        #region Variables
        // A list of the steps in the wave.
        [Tooltip("A list of the steps in the wave.")]
        [Header("Wave Information")]
        public List<Step> steps;
        #endregion
    }
    #endregion

    #region Step Class
    /// <summary>
    /// This class provides the basic data structure for creating steps.
    /// </summary>
    [System.Serializable]
    public class Step
    {
        #region StepTimingData Struct
        [System.Serializable]
        public struct StepTimingData
        {
            // How many seconds until the next step starts.
            [Tooltip("How many seconds until the next step starts.")]
            public float timeUntilNextStep;

            // The amount of seconds in betweeen spawned enemies.
            [Tooltip("The amount of seconds in betweeen spawned enemies.")]
            public float spacing;

            // Do we want to wait until the enemies are killed before moving onto the next step?
            [Tooltip("Do we want to wait until the enemies are killed before moving onto the next step?")]
            public bool waitUntilKill;
        }
        #endregion

        #region StepEnemyData Struct
        [System.Serializable]
        public struct StepEnemyData
        {
            // The type of enemy that we want to spawn.
            [Tooltip("The type of enemy that we want to spawn.")]
            public EnemyTypes enemy;

            // The maximum number of enemies we want to spawn in this step.
            [Tooltip("The maximum number of enemies we want to spawn in this step.")]
            public int maxNumberOfEnemies;

            // The minimum number of enemies we want to spawn in this step.
            [Tooltip("The minimum number of enemies we want to spawn in this step.")]
            public int minNumberOfEnemies;
        }
        #endregion

        #region Variables
        // The timing data for the current step.
        [Tooltip("The timing data for the current step.")]
        public StepTimingData timingData;

        // The enemy data for the current step.
        [Tooltip("The timing data for the current step.")]
        public StepEnemyData enemyData;

        // The chance that this step will be skipped entirely as a percentage.
        [Tooltip("// The chance that this step will be skipped entirely as a percentage.")]
        public int skipChance;
        #endregion
    }
    #endregion
}
