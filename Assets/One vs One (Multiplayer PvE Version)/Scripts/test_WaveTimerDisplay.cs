using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_WaveTimerDisplay : MonoBehaviour
{

    public Spawner spawner;
    Text waveText;

    void Start()
    {
        waveText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        waveText.text = spawner.WavesLeft.ToString();
    }
}
