using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Main : MonoBehaviour
{
    Animator attack_Animation;

    // Start is called before the first frame update
    void Start()
    {
        attack_Animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {   
        // One hand sword attack
        if (Input.GetMouseButtonDown(0)) attack_Animation.SetTrigger("leftmousebutton_press");
        
        // other animations
    }
}