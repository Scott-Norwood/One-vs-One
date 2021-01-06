using UnityEngine;
using System.Collections;
using xtilly5000.Prototypes.RecoilManager;
using GD.MinMaxSlider;

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
    [MinMaxSlider(0, 3)]
    public Vector2 weaponAudioPitch;

    [Space]
    [Header("MenuUI")]
    public ActivateMenu menuUiActivate; //Quick and dirty way to do this, reimplement later
    public ExitGame exitGame;

    private System_Recoil_Core recoilManager;
    private bool inBurst;

    private void Start()
    {
        recoilManager = gameObject.GetComponent<System_Recoil_Core>();
    }

    void Update()
    {

        if (Input.GetKeyDown(fireInput) && !menuUiActivate.mainMenuTab.activeSelf && !exitGame.exitConfirmation.activeSelf) //Goes along with the quick and dirty comment above
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
    }

    void WeaponMuzzleFlash()
    {
        // #### Implement LeanPool for this ####

        GameObject mF = Instantiate(weapon_Projectile_SO.projectileMuzzleFlash, muzzleEnd.position, muzzleEnd.rotation) as GameObject;
        mF.transform.SetParent(muzzleEnd);
    }

    void WeaponShellEjection()
    {
        // #### Implement LeanPool for this ####

        GameObject sE = Instantiate(weapon_Projectile_SO.projectileShellEffect, shellEjection.position, shellEjection.rotation) as GameObject;
        sE.transform.SetParent(shellEjection);
    }
    void WeaponSfx()
    {
        weaponAudio.pitch = Random.Range(weaponAudioPitch.x, weaponAudioPitch.y);
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
            yield return new WaitForSeconds(1f / (rateOfFire / 60f));
        }
    }
}