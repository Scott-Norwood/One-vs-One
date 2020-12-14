using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Turret : MonoBehaviour
{

    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //|| Weapon_Turret Variables
    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Header("General")]
    public float range = 15f;
    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Weapon_Projectile_SO weapon_Projectile_SO;

    public float rateOfFire;
    public AudioSource weaponAudio;
    public AudioClip weaponAudioClip;
    public float fireRate = 1f;
    [Space]
    [Header("Weapon Parts")]
    public Transform muzzleEnd;
    public Transform shellEjection;
    public Transform weaponBarrel;


    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //|| Private Variables
    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Transform target;
    int projectileVelocity;
    Rigidbody projectilePrefab;
    float fireCountdown = 0f;

    void Start()
    {
        //projectileVelocity = weapon_Projectile_SO.projectileVelocity;
        //projectilePrefab = weapon_Projectile_SO.projectilePrefab;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            LockOnTarget();
            if (fireCountdown <= 0f)
            {
                SpawnProjectile();
                WeaponSfx();
                WeaponMuzzleFlash();
                WeaponShellEjection();
                fireCountdown = 1f / fireRate;
            }
            RotateBarrel();
            fireCountdown -= Time.deltaTime;
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    private GameObject SpawnProjectile()
    {
        Rigidbody projectileInstance;
        projectileInstance = Lean.Pool.LeanPool.Spawn(weapon_Projectile_SO.projectilePrefab, muzzleEnd.position, muzzleEnd.rotation) as Rigidbody;
        projectileInstance.AddForce(muzzleEnd.forward * weapon_Projectile_SO.projectileVelocity);
        return projectileInstance.gameObject;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void WeaponSfx()
    {
        weaponAudio.PlayOneShot(weapon_Projectile_SO.projectileAudioClip);
    }
    void WeaponMuzzleFlash()
    {
        // #### Implement LeanPool for this ####

        GameObject mF = Lean.Pool.LeanPool.Spawn(weapon_Projectile_SO.projectileMuzzleFlash, muzzleEnd.position, muzzleEnd.rotation) as GameObject;
        mF.transform.SetParent(muzzleEnd);
    }

    void WeaponShellEjection()
    {
        // #### Implement LeanPool for this ####

        GameObject sE = Lean.Pool.LeanPool.Spawn(weapon_Projectile_SO.projectileShellEffect, shellEjection.position, shellEjection.rotation) as GameObject;
        sE.transform.SetParent(shellEjection);
    }
    void RotateBarrel()
    {
        weaponBarrel.transform.Rotate(Vector3.forward, 500 * Time.deltaTime);
    }

}
