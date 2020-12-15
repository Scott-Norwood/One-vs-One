using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class System_Currency_Core : MonoBehaviour
{
    public int value;
    int worth;
    [ReadOnly] [SerializeField] int storedCurrency;
    [ReadOnly] [SerializeField] int maxCurrency = 999999;

    public virtual void IncreasePoints(int worth)
    {
        storedCurrency += worth;
        if (storedCurrency >= maxCurrency) { storedCurrency = maxCurrency; }
    }

    public virtual void DecreasePoints()
    {
        storedCurrency -= value;
        if (storedCurrency <= 0) { storedCurrency = 0; }
    }

    public virtual int GetCurrentCurrency()
    {
        return storedCurrency;
    }
    public virtual int GetWorth()
    {
        return (worth = value);
    }
}
