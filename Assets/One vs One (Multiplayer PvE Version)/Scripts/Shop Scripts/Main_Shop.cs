using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using TMPro;
using xtilly5000.Prototypes.WaveManager;

public class Main_Shop : MonoBehaviour
{
    [Header("Weapon Shop Buff Stats")]
    public int PointsCost;
    public int WeaponDamageIncrease;
    public float WeaponSpeedIncrease;
    public float WeaponReloadIncrease;
    public bool WeaponMagazineBased;

    [Header("Shop Item Values")]
    public float BuffDuration;

    //Hidden Values 
    Canvas mainShopCanvas;
    bool isShopOpen;
    bool timerIsRunning = false;
    int currentPoints;
    TMP_Text buffTimer;


    void Start()
    {
        mainShopCanvas = GetComponent<Canvas>();
        currentPoints = GameManager.Instance.Points;
        timerIsRunning = false;
        //mainShopCanvas.enabled = false;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (BuffDuration > 0)
            {
                BuffDuration -= Time.deltaTime;
            }
            else
            {

                BuffDuration = 0;
                timerIsRunning = false;
            }
        }
    }

    #region All shop button methods go in here
    public void WeaponDamageButton()
    {
        if (currentPoints >= 500)
        {
            GameManager.Instance.SetPoints(currentPoints -= 500);
            timerIsRunning = true;
        }
        else
        {
            timerIsRunning = false;
            return;
        }
        MMEventManager.TriggerEvent(new MMGameEvent("WeaponDamageIncrease"));
    }

    public void WeaponSpeedButton()
    {
        if (currentPoints >= 500)
        {
            GameManager.Instance.SetPoints(currentPoints -= 500);
            timerIsRunning = true;
        }
        else
        {
            timerIsRunning = false;
            return;
        }
        MMEventManager.TriggerEvent(new MMGameEvent("WeaponSpeedIncrease"));
    }

    public void WeaponReloadButton()
    {
        if (currentPoints >= 500)
        {
            GameManager.Instance.SetPoints(currentPoints -= 500);
            timerIsRunning = true;
        }
        else
        {
            timerIsRunning = false;
            return;
        }
        MMEventManager.TriggerEvent(new MMGameEvent("WeaponReloadIncrease"));
    }

    public void WeaponMagazineButton()
    {
        GameManager.Instance.SetPoints(currentPoints -= 500);
        timerIsRunning = true;

        MMEventManager.TriggerEvent(new MMGameEvent("WeaponMagazineIncrease"));
    }

    public void AddPointsDebug()
    {
        MoreMountains.CorgiEngine.GameManager.Instance.AddPoints(10000);
    }

    #endregion
}
