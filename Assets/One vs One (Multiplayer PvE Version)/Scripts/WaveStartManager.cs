using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xtilly5000.Prototypes.WaveManager;

public class WaveStartManager : MonoBehaviour
{
    
    void Start()
    {
        WaveManager.Instance.SpawnWave(0);
    }
}
