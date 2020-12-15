using UnityEngine;
using TMPro;
using Michsky.UI.ModernUIPack;

public class NameToText : MonoBehaviour
{
    ButtonManagerBasic buttonManagerBasic;

    void Start()
    {
        buttonManagerBasic = GetComponent<ButtonManagerBasic>();
        buttonManagerBasic.buttonText = gameObject.name;
    }
}
