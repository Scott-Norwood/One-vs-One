using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public GameObject exitConfirmation;
    public GameObject shopMenu;
    void Start()
    {
        exitConfirmation.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !shopMenu.activeSelf)
        {
            exitConfirmation.SetActive(!exitConfirmation.activeSelf);
            PauseManager.pauseManager.isPaused = !PauseManager.pauseManager.isPaused;
        }
    }

    public void ExitWithoutFeedback()
    {
        Application.Quit();
    }
    public void ExitWithFeedback()
    {
        Application.OpenURL("https://forms.gle/q46T73vCaWB8JJgU7");
        Application.Quit();
    }
}
