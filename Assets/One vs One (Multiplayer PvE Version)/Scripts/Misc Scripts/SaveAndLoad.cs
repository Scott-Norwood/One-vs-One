using UnityEngine;
using MoreMountains.CorgiEngine;
using UnityEngine.SceneManagement;
using xtilly5000.Prototypes.WaveManager;
using System.Collections;

public class SaveAndLoad : MonoBehaviour
{

    WaveSpawner WaveSpawner;
    public static SaveAndLoad i_SaveAndLoad;
    WaitForSecondsRealtime delay = new WaitForSecondsRealtime(1);

    void Awake() // In this method were setting up a reference and loading in the save data before the scene loads
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Load();
        }
        else
        {
            return;
        }

        StartCoroutine(AutoSave());
    }

    private void Load()
    {
        // If the scene we're on isn't the mainmenu, we then load the relevent data
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            WaveSpawner = FindObjectOfType<WaveSpawner>();

            GameManager.Instance.SetPoints(ES3.Load("GamePoints", GameManager.Instance.Points));
            GameManager.Instance.CurrentLives = ES3.Load("GameMaxLives", GameManager.Instance.MaximumLives);
            WaveSpawner.currentAttemptedWave = ES3.Load("CurrentWaveAttempt", WaveSpawner.currentAttemptedWave);
        }
    }

    public void ResetSettingsData() // Not yet implemented.
    {
        //Reset game settings here.
    }

    public void SaveSettingsData() // Not yet implemented.
    {
        //Save game settings here.
    }

    public void StartNewGame() // Literally deletes the save file, 100% delete.
    {
        ES3.DeleteFile();
    }

    public void ContinueNewGame() // Not yet implemented.
    {
        //Do stuff here.
    }

    IEnumerator AutoSave() // Waits X second then auto saves data listed within
    {
        while (GameManager.Instance.CurrentLives >= 0 && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 0)
        {
            ES3.Save("GamePoints", GameManager.Instance.Points);
            ES3.Save("GameMaxLives", GameManager.Instance.MaximumLives);
            ES3.Save("CurrentWaveAttempt", WaveSpawner.currentAttemptedWave);

            yield return delay;

            print("| GamePoints " + GameManager.Instance.Points + " |" +
            "| GameMaxLives " + GameManager.Instance.MaximumLives + " |" +
            "| CurrentWaveAttempt " + WaveSpawner.currentAttemptedWave + " |");
        }
    }
}