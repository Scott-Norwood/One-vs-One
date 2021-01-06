using UnityEngine;

public class DamageOverTimeHandler : MonoBehaviour
{

    //! Hard coded values in the damageovertime script, fix later

    public ParticleSystem dotEffect;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy_Base" && other.gameObject.GetComponent<DamageOverTime>() == null)
        {
            other.gameObject.AddComponent<DamageOverTime>();
            Instantiate(dotEffect, other.transform.position, Quaternion.Euler(270, 0, 0), other.transform);
        }
    }
}
