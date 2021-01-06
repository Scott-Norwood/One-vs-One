using System.Collections;
using UnityEngine;

public class Weapon_Projectile_Hit : MonoBehaviour
{
    public Weapon_Projectile_SO weapon_Projectile_SO;
    void Update()
    {
        Destroy(gameObject, .25f);
    }
    void OnCollisionEnter(Collision other)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(LetVFXFinishBullet()); 
    }

    IEnumerator LetVFXFinishBullet()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
