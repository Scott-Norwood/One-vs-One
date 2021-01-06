using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_UIFAdeIn : MonoBehaviour
{
    public Animator animator;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            animator.Play("MainMenu_UIFadeIn", 0);
        }
    }
}
