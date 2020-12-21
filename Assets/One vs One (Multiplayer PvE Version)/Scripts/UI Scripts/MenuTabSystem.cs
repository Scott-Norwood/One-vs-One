using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTabSystem : MonoBehaviour
{
    public GameObject TabSystem;
    public GameObject mainTab;

    public void GoToTab()
    {
        mainTab.SetActive(false);
        TabSystem.SetActive(true);
    }
    public void GoHome()
    {
        TabSystem.SetActive(false);
        mainTab.SetActive(true);
    }
    public void ClickedDebug()
    {
        Debug.Log("Clicked");
    }

}
