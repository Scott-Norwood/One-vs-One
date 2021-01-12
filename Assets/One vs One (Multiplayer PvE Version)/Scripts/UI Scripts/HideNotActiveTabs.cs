using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNotActiveTabs : MonoBehaviour
{
    public GameObject[] menuTabs;

    public void ShowTab(int tabindex)
    {
        HideAllButActiveTabs();
        menuTabs[tabindex].SetActive(true);
    }

    public void HideAllButActiveTabs()
    {
        for (int i = 0; i < menuTabs.Length; i++)
        {
            menuTabs[i].SetActive(false);
        }
    }
}
