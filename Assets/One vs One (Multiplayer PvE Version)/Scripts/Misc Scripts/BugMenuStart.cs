using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG;

public class BugMenuStart : MonoBehaviour
{
    public GameObject toolUI;
    public UIManager uIManager;
    bool keyToggle;
    void Start()
    {
        toolUI.SetActive(false);
        keyToggle = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12) && keyToggle == false)
        {
            keyToggle = true;
            toolUI.SetActive(true);
            uIManager.TakeScreenshotButton();
            uIManager.PauseGame();
            Cursor.visible = true;
        }
    }
}


