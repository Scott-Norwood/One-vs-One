using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using xtilly5000.Prototypes.WaveManager;

public class GUIManager : MonoBehaviour
{
    public GameObject _waveTextGameobject;
    TMP_Text _waveText;
    Animator _waveTextAnimator;
    WaveSpawner WaveSpawner;

    void Start()
    {
        // References set here:
        _waveText = _waveTextGameobject.GetComponent<TMP_Text>();
        _waveTextAnimator = _waveTextGameobject.GetComponent<Animator>();
        WaveSpawner = FindObjectOfType<WaveSpawner>();
        WaveManager.OnWaveKilled += OnWaveKilled;

        // Setup here:
        _waveText.text = "Wave: " + (WaveSpawner.currentWave);
        _waveTextAnimator.SetTrigger("textTrigger");

    }



    // Updates the _waveText with the current wave index plus 1 to make display the correct wave number
    private void OnWaveKilled(Wave wave)
    {
        _waveText.text = "Wave: " + (WaveSpawner.currentWave);
        _waveTextAnimator.SetTrigger("textTrigger");
    }
}
