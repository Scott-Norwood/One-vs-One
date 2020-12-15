using UnityEngine;
using System.Collections;
using Lean.Pool;
using xtilly5000.Prototypes.RecoilManager;

public enum FireSelect
{
    SingleFire,
    BurstFire,
    FullAuto
}

public class Weapon_Shoot : MonoBehaviour
{
    public FireSelect _fireselect;

    [Space]
    [Header("Weapon Settings")]
    public KeyCode fireInput;
    public float rateOfFire;
    public int roundsInBurst;

    [Space]
    [Header("Weapon Projectile")]
    public Weapon_Projectile_SO weapon_Projectile_SO;

    [Space]
    [Header("Weapon Parts")]
    public Transform muzzleEnd;
    public Transform shellEjection;

    [Space]
    [Header("Weapon SFX")]
    public AudioSource weaponAudio;

    private System_Recoil_Core recoilManager;
    private bool inBurst;

    private void Start()
    {
        recoilManager = gameObject.GetComponent<System_Recoil_Core>();
    }

    void Update()
    {
        if (Input.GetKeyDown(fireInput))
        {
            //##### Firemodes #####
            if (_fireselect == FireSelect.SingleFire)
            {
                SingleFire();
            }
            if (_fireselect == FireSelect.BurstFire && !inBurst)
            {
                StartCoroutine(BurstFire());
            }
            if (_fireselect == FireSelect.FullAuto)
            {
                StartCoroutine(FullAuto());
            }
        }
        if (!Input.GetKeyDown(fireInput))
        {
            //recoilAngle = 0;
        }
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
    void WeaponSfx()
    {
        weaponAudio.PlayOneShot(weapon_Projectile_SO.projectileAudioClip);
    }

    private GameObject SpawnProjectile()
    {
        Rigidbody projectileInstance;
        projectileInstance = Instantiate(weapon_Projectile_SO.projectilePrefab, muzzleEnd.position, muzzleEnd.rotation) as Rigidbody;
        projectileInstance.AddForce(muzzleEnd.forward * weapon_Projectile_SO.projectileVelocity);
        return projectileInstance.gameObject;
    }

    public virtual void SingleFire()
    {
        SpawnProjectile();
        WeaponMuzzleFlash();
        WeaponShellEjection();
        WeaponSfx();
        recoilManager.Recoil();
        print(_fireselect);
    }

    public virtual IEnumerator BurstFire()
    {
        inBurst = true;
        int _roundsLeftInBurst = roundsInBurst;
        while (_roundsLeftInBurst > 0)
        {
            SingleFire();
            recoilManager.Recoil();
            _roundsLeftInBurst--;
            print(_fireselect);
            yield return new WaitForSeconds(1f / (rateOfFire / 60f));
        }
        yield return new WaitForSeconds(1f / (rateOfFire / 60f));
        inBurst = false;
    }

    public virtual IEnumerator FullAuto()
    {
        while (Input.GetKey(fireInput))
        {
            SingleFire();
            recoilManager.Recoil();
            print(_fireselect);
            yield return new WaitForSeconds(1f / (rateOfFire / 60f));
        }
    }
}