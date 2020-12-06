using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Shop/Buffs")]
public class BuffsScriptableObject : ScriptableObject
{
    [Header("Weapon Shop Buff Stats")]
    public int PointsCost;
    public int WeaponDamageIncrease;
    public float WeaponSpeedIncrease;
    public float WeaponReloadIncrease;
    public bool WeaponMagazineBased;
    public float BuffDuration;

}
