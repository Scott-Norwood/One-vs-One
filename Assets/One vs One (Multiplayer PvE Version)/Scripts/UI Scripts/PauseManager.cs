using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused;
    public static PauseManager pauseManager;
    void Start()
    {
        pauseManager = this;
        isPaused = false;
    }

    void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!isPaused)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
