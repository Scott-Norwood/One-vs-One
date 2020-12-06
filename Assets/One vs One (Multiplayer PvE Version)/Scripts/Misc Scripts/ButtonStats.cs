using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStats : MonoBehaviour
{
    public BuffsScriptableObject buffsScriptableObject;
    Button button;
    Main_Shop main_Shop;

    void Start()
    {
        main_Shop = FindObjectOfType<Main_Shop>();

    }

    void PassStats()
    {
        main_Shop.BuffDuration = buffsScriptableObject.BuffDuration;
        main_Shop.WeaponDamageIncrease = buffsScriptableObject.WeaponDamageIncrease;
        main_Shop.WeaponReloadIncrease = buffsScriptableObject.WeaponReloadIncrease;
        main_Shop.WeaponSpeedIncrease = buffsScriptableObject.WeaponSpeedIncrease;
        main_Shop.WeaponMagazineBased = buffsScriptableObject.WeaponMagazineBased;
    }
}
