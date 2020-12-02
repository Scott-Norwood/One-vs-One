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
    }
}
