using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using xtilly5000.Prototypes.WaveManager;

public class GUIManager : MonoBehaviour
{

    public TMP_Text _waveText;
    WaveSpawnerTest waveSpawnerTest;

    // Start is called before the first frame update
    void Start()
    {
        waveSpawnerTest = FindObjectOfType<WaveSpawnerTest>();
        WaveManager.OnWaveKilled += OnWaveKilled;
        _waveText.text = "Wave: " + (waveSpawnerTest.currentWave + 1f);
    }

    private void OnWaveKilled(Wave wave)
    {
        _waveText.text = "Wave: " + (waveSpawnerTest.currentWave + 2f);
    }
}
