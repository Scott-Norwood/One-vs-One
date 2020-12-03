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
    [CreateAssetMenu(fileName = "New Wave", menuName = "Wave Manager/New Wave")]
    public class Wave : ScriptableObject
    {
        #region Variables
        // A list of the steps in the wave.
        [Tooltip("A list of the steps in the wave.")]
        [Header("Wave Information")]
        public List<Step> steps = new List<Step>()
        {
            new Step()
            {
                // We do not want any information in the newly created step, so leave this blank.
            }
        };
        #endregion
    }
    #endregion
}
