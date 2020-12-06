using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    public Animator animator;
    public float lookIkWeight;
    public float bodyWeight;
    public float headWeight;
    public float eyesWeight;
    public float clampWeight;

    public GameObject _gameObject;
    public Transform lookPos;

    void OnAnimatorIK()
    {
        _gameObject = GameObject.FindGameObjectWithTag("Weapon");
        lookPos = _gameObject.transform;
        animator.SetLookAtWeight(lookIkWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
        animator.SetLookAtPosition(lookPos.position);
    }

}
