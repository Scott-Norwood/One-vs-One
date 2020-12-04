using UnityEngine;
using MoreMountains.CorgiEngine;
using UnityEngine.SceneManagement;
using xtilly5000.Prototypes.WaveManager;

public class SaveAndLoad : MonoBehaviour
{
    WaveSpawner WaveSpawner;
    public static SaveAndLoad i_SaveAndLoad;

    // In this method were setting up a reference and loading in the save data before the scene loads
    void Awake()
    {
        // Checks if we have a savemanager in the scene and if not, adds one, if so, destroys this.
        if (i_SaveAndLoad == null)
        {
            i_SaveAndLoad = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneLoaded;
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

            Debug.Log(WaveSpawner.currentAttemptedWave);
            Debug.Log("Loading");
        }
    }

    private void OnSceneLoaded(Scene scene, Scene next)
    {
        Load();
    }

    //Constantly updating the save data, if the player dies it saves their data, along with if they want to quit it also saves
    void Update()
    {
        if (WaveSpawner == null)
        {
            return;
        }

        //If the players lives are currently 0, then the gamemanager reloads the scene and we save our points before that happens
        if (GameManager.Instance.CurrentLives == 0 && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 0)
        {
            ES3.Save("GamePoints", GameManager.Instance.Points);
            ES3.Save("GameMaxLives", GameManager.Instance.MaximumLives);
            ES3.Save("CurrentWaveAttempt", WaveSpawner.currentAttemptedWave);
            Debug.Log("Saving");
        }
        else
        {
            return;
        }
        // The application is quitting, like in anyway, altf4, or normally, we call WantsToQuit method
        //Application.quitting += WantsToQuit;
    }

    private void OnApplicationQuit()
    {
        WantsToQuit();
    }


    public void ResetSettingsData()
    {
        //Reset game settings here.
    }


    public void SaveSettingsData()
    {
        //Save game settings here.
    }

    // Literally deletes the save file, 100% delete.
    public void StartNewGame()
    {
        ES3.DeleteFile();
    }


    public void ContinueNewGame()
    {
        //Do stuff here.
    }

    // When the player wants to quit we save all game data in here
    void WantsToQuit()
    {
        if (GameManager.Instance.CurrentLives == 0 && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 0)
        {
            ES3.Save("GamePoints", GameManager.Instance.Points);
            ES3.Save("CurrentWaveAttempt", WaveSpawner.currentAttemptedWave);
        }
    }
}
