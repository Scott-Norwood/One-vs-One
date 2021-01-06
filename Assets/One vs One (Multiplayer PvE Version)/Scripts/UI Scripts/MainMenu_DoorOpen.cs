using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_DoorOpen : MonoBehaviour
{
    Animator animator;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            animator = GetComponent<Animator>();
            animator.Play("MainMenu_DoorOpen", 0);
        }
    }
}

