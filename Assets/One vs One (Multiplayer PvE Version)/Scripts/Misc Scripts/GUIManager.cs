using UnityEngine;
using UnityEngine.UI;
using TMPro;
using xtilly5000.Prototypes.WaveManager;

public class GUIManager : MonoBehaviour
{
    public GameObject _waveTextGameobject;
    public Canvas shopCanvas;
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
        Debug.Log("OnWaveKilled called.");
        _waveText.text = "Wave: " + (WaveSpawner.currentWave);
        _waveTextAnimator.SetTrigger("textTrigger");

        // Checks if were on X index (remember to subtract wave-1, to get the index) and then if true, opens the shop tab
        if (WaveSpawner.currentAttemptedWave == 9)
        {
            // Do shop tab code opening thing here
            Debug.Log("We made it to wave 10, index 9, opening shop tab.");
        }
    }
}
