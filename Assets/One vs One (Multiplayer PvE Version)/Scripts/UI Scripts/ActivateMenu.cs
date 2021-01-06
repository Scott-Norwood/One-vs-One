using UnityEngine;

public class ActivateMenu : MonoBehaviour
{
    public KeyCode input;
    public GameObject mainMenuTab;
    public GameObject escapeMenu;

    void Start()
    {
        mainMenuTab.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(input) && !escapeMenu.activeSelf)
        {
            mainMenuTab.SetActive(!mainMenuTab.activeSelf);
            PauseManager.pauseManager.isPaused = !PauseManager.pauseManager.isPaused;
        }
    }
}

