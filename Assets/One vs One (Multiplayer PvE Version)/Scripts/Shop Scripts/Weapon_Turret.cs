﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Turret : MonoBehaviour
{
    Transform target;
    [Header("General")]
    public float range = 15f;
    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Weapon_Projectile_SO weapon_Projectile_SO;
    public Transform muzzleEnd;
    public float rateOfFire;

    // Use this for initialization
    void Start()
    {
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
        LockOnTarget();
        if (target != null)
        {
            SpawnProjectile();
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
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

    IEnumerator FullAuto()
    {
        while (target != null)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(1f / (rateOfFire / 60f));
        }
    }
}