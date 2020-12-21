using UnityEngine;
using System.Collections;
using xtilly5000.Prototypes.RecoilManager;
using GD.MinMaxSlider;
using TMPro;

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

    private System_Recoil_Core recoilManager;
    private bool inBurst;
    [Space]
    [Header("For Testing Only")]
    public KeyCode debugMenu;
    public bool testMode;
    public GameObject debugCanvas;
    public TMP_Text burstText;
    public TMP_Text roundsFiredText;
    public TMP_Text roundsOnTargetText;
    [ReadOnly] public int roundsOnTarget;
    [SerializeField] [ReadOnly] int roundsFired;

    private void Start()
    {
        recoilManager = gameObject.GetComponent<System_Recoil_Core>();
        burstText.text = roundsInBurst.ToString();
        roundsFiredText.text = roundsFired.ToString();
        roundsOnTargetText.text = roundsOnTarget.ToString();
    }

    void Update()
    {
        if (testMode == true)
        {
            debugCanvas.SetActive(true);
        }
        else
        {
            debugCanvas.SetActive(false);
        }

        if (!menuUiActivate.menuTab.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(fireInput) && !menuUiActivate.menuTab.activeSelf) //Goes along with the quick and dirty comment above
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
        roundsOnTargetText.text = roundsOnTarget.ToString();
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
        print(_fireselect);
        roundsFired++;
        roundsFiredText.text = roundsFired.ToString();
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
        if (testMode == true)
        {
            //roundsInBurst++;
        }

        burstText.text = roundsInBurst.ToString();
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