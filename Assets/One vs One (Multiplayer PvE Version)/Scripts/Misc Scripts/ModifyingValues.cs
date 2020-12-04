using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.MMInterface;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

public class ModifyingValues : MonoBehaviour, MMEventListener<MMGameEvent>
{
    HitscanWeapon weapon;
    Main_Shop main_Shop;
    int _prebuffWeaponDamage;
    float _prebuffWeaponSpeed;
    float _prebuffReloadSpeed;
    bool _prebuffMagazineBased;
    float _buffDuration;


    void Start()
    {
        weapon = GetComponent<HitscanWeapon>();
        main_Shop = FindObjectOfType<Main_Shop>();
        //characterHandleWeapon = FindObjectOfType<CharacterHandleWeapon>();

        _prebuffWeaponDamage = weapon.DamageCaused;
        _prebuffWeaponSpeed = weapon.TimeBetweenUses;
        _prebuffReloadSpeed = weapon.ReloadTime;
        _prebuffMagazineBased = weapon.MagazineBased;
        _buffDuration = main_Shop.BuffDuration;
    }

    void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }
    void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public virtual void OnMMEvent(MMGameEvent gameEvent)
    {
        if (gameEvent.EventName == "WeaponDamageIncrease")
        {
            weapon.DamageCaused += main_Shop.WeaponDamageIncrease;
            Debug.Log("Weapon Damage was: " + _prebuffWeaponDamage + ", now: " + weapon.DamageCaused + " | Increased by: " + main_Shop.WeaponDamageIncrease + ".");
            StartCoroutine(CountDown());
        }
        if (gameEvent.EventName == "WeaponSpeedIncrease")
        {
            weapon.TimeBetweenUses -= main_Shop.WeaponSpeedIncrease;
            Debug.Log("Weapon ROF was: " + _prebuffWeaponSpeed + "s, now: " + weapon.TimeBetweenUses + "s | Decreased by: " + main_Shop.WeaponSpeedIncrease + "s.");
            StartCoroutine(CountDown());
        }
        if (gameEvent.EventName == "WeaponReloadIncrease")
        {
            weapon.ReloadTime -= main_Shop.WeaponReloadIncrease;
            Debug.Log("Weapon Reload Speed was: " + _prebuffReloadSpeed + "s, now: " + weapon.ReloadTime + "s | Decreased by: " + main_Shop.WeaponReloadIncrease + "s.");
            StartCoroutine(CountDown());
        }
        if (gameEvent.EventName == "WeaponMagazineIncrease")
        {
            weapon.MagazineBased = main_Shop.WeaponMagazineBased = false;
            Debug.Log("Weapon Infinite Ammo was: " + _prebuffMagazineBased + ", now: " + weapon.MagazineBased + ".");
            StartCoroutine(CountDown());
        }
        Debug.Log("We've listened to your request");
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(_buffDuration);
        weapon.DamageCaused = _prebuffWeaponDamage;
        weapon.TimeBetweenUses = _prebuffWeaponSpeed;
        weapon.ReloadTime = _prebuffReloadSpeed;
        weapon.MagazineBased = _prebuffMagazineBased;
        Debug.Log("Reset all stats.");
    }
}
