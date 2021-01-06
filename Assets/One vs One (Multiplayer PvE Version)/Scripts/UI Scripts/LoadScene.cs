using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public int sceneIndex;
    public float loadsceneTransitionTime;
    public TMP_Text startText;
    public GameObject loadingBar;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(ReadyToStart());
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(WaitBeforeLoad());
        }
    }

    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //|| Loading Screen
    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator WaitBeforeLoad()
    {
        yield return new WaitForSecondsRealtime(loadsceneTransitionTime);
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
    }
    IEnumerator ReadyToStart()
    {
        yield return new WaitForSecondsRealtime(2);
        startText.SetText("Press space to start...", true);
    }

    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //|| Main Menu
    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void MainMenuStartButton()
    {
        StartCoroutine(WaitBeforeLoad());
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
