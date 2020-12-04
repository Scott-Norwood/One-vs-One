using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using xtilly5000.Prototypes.WaveManager;

public class GUIManager : MonoBehaviour
{

    public TMP_Text _waveText;
    WaveSpawner WaveSpawner;

    // Start is called before the first frame update
    void Start()
    {
        WaveSpawner = FindObjectOfType<WaveSpawner>();
        WaveManager.OnWaveKilled += OnWaveKilled;
        _waveText.text = "Wave: " + (WaveSpawner.currentWave + 1f);
    }

    private void OnWaveKilled(Wave wave)
    {
        _waveText.text = "Wave: " + (WaveSpawner.currentWave + 2f);
    }
}
