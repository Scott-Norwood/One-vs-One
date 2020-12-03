using UnityEngine;
using MoreMountains.CorgiEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{

    int _gamePoints;
    int _gameMaxLives;

    // In this method were setting up a reference and loading in the save data before the scene loads
    void Awake()
    {
        // If the scene we're on isn't the mainmenu, we then load the relevent data
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            _gamePoints = ES3.Load("GamePoints", GameManager.Instance.Points);
            _gameMaxLives = ES3.Load("GameMaxLives", GameManager.Instance.MaximumLives);

            GameManager.Instance.SetPoints(_gamePoints);
            GameManager.Instance.CurrentLives = _gameMaxLives;
            Debug.Log("Loading");
        }
    }

    //Constantly updating the save data, if the player dies it saves their data, along with if they want to quit it also saves
    void Update()
    {
        _gamePoints = GameManager.Instance.Points;

        //If the players lives are currently 0, then the gamemanager reloads the scene and we save our points before that happens
        if (GameManager.Instance.CurrentLives == 0 && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 0)
        {
            ES3.Save("GamePoints", GameManager.Instance.Points);
            ES3.Save("GameMaxLives", GameManager.Instance.MaximumLives);
            Debug.Log("Saving");
        }
        else
        {
            return;
        }
        // The application is quitting, like in anyway, altf4, or normally, we call WantsToQuit method
        Application.quitting += WantsToQuit;
    }


    public void ResetSettingsData()
    {
        //Reset game settings here.
    }


    public void SaveSettingsData()
    {
        //Save game settings here.
    }


    public void StartNewGame()
    {
        ES3.DeleteFile(Application.persistentDataPath);
        ES3.Save("GamePoints", GameManager.Instance.Points);
        ES3.Save("GameMaxLives", GameManager.Instance.MaximumLives);
    }


    public void ContinueNewGame()
    {
        //Do stuff here.
    }

    // When the player wants to quit we save all game data in here
    void WantsToQuit()
    {
        ES3.Save("GamePoints", GameManager.Instance.Points);
    }
}
