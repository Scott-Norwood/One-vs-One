using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProjectVersion : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Text _text = GetComponent<Text>();
        _text.text = Application.version.ToString();
    }
}
