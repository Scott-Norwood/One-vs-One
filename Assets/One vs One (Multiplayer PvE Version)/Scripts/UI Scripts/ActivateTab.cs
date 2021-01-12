using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTab : MonoBehaviour
{
    public GameObject tab;

    public void ShowTab()
    {
        tab.SetActive(!tab.activeSelf);
    }

    public void HideTab()
    {
        //! Empty
    }
}
