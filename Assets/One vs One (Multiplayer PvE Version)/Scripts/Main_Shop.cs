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
    [SerializeField] public int WeaponDamageIncrease;
    [SerializeField] public float WeaponSpeedIncrease;
    [SerializeField] public float WeaponReloadIncrease;
    [SerializeField] public bool WeaponMagazineBased;
    [SerializeField] public float BuffDuration;
    [SerializeField] TMP_Text buffTimer;

    //Hidden Values 
    [SerializeField, HideInInspector] Canvas mainShopCanvas;
    [SerializeField, HideInInspector] bool isShopOpen;
    [HideInInspector] bool timerIsRunning = false;
    [SerializeField, HideInInspector] int currentPoints;

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
                DisplayTime(BuffDuration);
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
        MMEventManager.TriggerEvent(new MMGameEvent("WeaponMagazineIncrease"));
    }
    public void AddPointsDebug()
    {
        MoreMountains.CorgiEngine.GameManager.Instance.AddPoints(currentPoints += 500);
    }

    #endregion

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        buffTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
