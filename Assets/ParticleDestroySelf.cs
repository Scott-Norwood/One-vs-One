using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroySelf : MonoBehaviour
{
    ParticleSystem fireParticleSystem;
    void Start()
    {
        fireParticleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(fireParticleSystem.main.duration);
        Destroy(gameObject);
    }
}
