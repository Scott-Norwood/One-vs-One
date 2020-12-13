using UnityEngine;
using System.Collections;
using Lean.Pool;

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

    [Space]
    [Header("Recoil Settings")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;
    //float recoilAngle;
    Vector3 gunStartingPosition;
    Vector3 recoilSmoothDampVelocity;
    Weapon_Aim aimScript;

    void Start()
    {
        gunStartingPosition = transform.localPosition;
        aimScript = GetComponent<Weapon_Aim>();
        print(1f / (rateOfFire / 60f));
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
            if (_fireselect == FireSelect.BurstFire)
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
    void LateUpdate()
    {
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, gunStartingPosition, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
    }
    void RecoilStart()
    {
        transform.position += transform.TransformDirection(Vector3.back) * Random.Range(kickMinMax.x, kickMinMax.y);

        //aimScript.targetAngle = aimScript.mouseAngle + Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
        aimScript.gameObject.transform.eulerAngles = new Vector3(aimScript.gameObject.transform.eulerAngles.x - Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y), aimScript.gameObject.transform.eulerAngles.y, aimScript.gameObject.transform.eulerAngles.z);
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
        projectileInstance = Lean.Pool.LeanPool.Spawn(weapon_Projectile_SO.projectilePrefab, muzzleEnd.position, muzzleEnd.rotation) as Rigidbody;
        projectileInstance.AddForce(muzzleEnd.forward * weapon_Projectile_SO.projectileVelocity);
        return projectileInstance.gameObject;
    }

    public virtual void SingleFire()
    {
        SpawnProjectile();
        WeaponMuzzleFlash();
        WeaponShellEjection();
        WeaponSfx();
        RecoilStart();
        print(_fireselect);
    }

    public virtual IEnumerator BurstFire()
    {
        int _roundsLeftInBurst = roundsInBurst;
        while (_roundsLeftInBurst > 0)
        {
            SingleFire();
            RecoilStart();
            _roundsLeftInBurst--;
            print(_fireselect);
            yield return new WaitForSeconds(1f / (rateOfFire / 60f));
        }
    }

    public virtual IEnumerator FullAuto()
    {
        while (Input.GetKey(fireInput))
        {
            SingleFire();
            RecoilStart();
            print(_fireselect);
            yield return new WaitForSeconds(1f / (rateOfFire / 60f));
        }
    }
}