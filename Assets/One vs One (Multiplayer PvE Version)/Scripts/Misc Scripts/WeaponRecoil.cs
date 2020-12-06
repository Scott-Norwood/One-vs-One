using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;


    [Space]
    [Header("Firing Settings")]
    public float fireRate;
    public float timeBetweenBursts;
    public int roundsInBurst;
    public bool burstMode;
    bool buttonHeldDown;

    // Private variables
    int roundsLeftInBurst;
    HitscanWeapon hitscanWeapon;
    Weapon weapon;
    Vector3 recoilSmoothDampVelocity;
    [SerializeField] int weaponAmmoLeftInMag;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;
    bool isActive;


    void Start()
    {
        hitscanWeapon = FindObjectOfType<HitscanWeapon>();
        timeBetweenBursts = hitscanWeapon.TimeBetweenUses - (fireRate * roundsInBurst);
        isActive = false;
        burstMode = false;
        buttonHeldDown = false;
    }

    void Update()
    {
        weaponAmmoLeftInMag = hitscanWeapon.CurrentAmmoLoaded;

        if (InputManager.Instance.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonDown && isActive == false)
        {
            timeBetweenBursts = hitscanWeapon.TimeBetweenUses - (fireRate * roundsInBurst);
            isActive = true;
            buttonHeldDown = true;

            if (burstMode == true && weaponAmmoLeftInMag > 0)
            {
                StartCoroutine(BurstFire());
            }

            if (burstMode == false)
            {
                if (buttonHeldDown == true)
                {
                    StartCoroutine(AutoFire());
                }
            }
        }

        if (InputManager.Instance.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonUp)
        {
            buttonHeldDown = false;
        }
    }

    void LateUpdate()
    {
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
    }

    void Shoot()
    {
        // Initiate Recoil
        transform.localPosition -= Vector3.right * Random.Range(kickMinMax.x, kickMinMax.y);
        recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
        recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
    }

    IEnumerator BurstFire()
    {
        roundsLeftInBurst = roundsInBurst;
        while (roundsLeftInBurst > 0)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
            roundsLeftInBurst--;
        }
        yield return new WaitForSeconds(timeBetweenBursts);
        isActive = false;
    }
    IEnumerator AutoFire()
    {
        while (buttonHeldDown == true && weaponAmmoLeftInMag > 0)
        {
            Shoot();
            yield return new WaitForSeconds(hitscanWeapon.TimeBetweenUses);
        }
        isActive = false;
    }
}
