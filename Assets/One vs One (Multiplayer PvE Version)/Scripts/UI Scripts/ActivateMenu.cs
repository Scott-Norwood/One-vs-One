using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMenu : MonoBehaviour
{
    public KeyCode input;
    public GameObject menuTab;
    //public AudioSource menuOpeningSound;

    void Start()
    {
        menuTab.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(input))
        {
            menuTab.SetActive(!menuTab.activeSelf);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

