using UnityEngine;

public class System_Currency_Core : MonoBehaviour
{
    public int value;
    int worth;
    [ReadOnly] [SerializeField] int storedCurrency;
    [ReadOnly] [SerializeField] int maxCurrency = 999999;

    public void IncreasePoints(int worth)
    {
        storedCurrency += worth;
        if (storedCurrency >= maxCurrency) { storedCurrency = maxCurrency; }
    }

    public void DecreasePoints()
    {
        storedCurrency -= value;
        if (storedCurrency <= 0) { storedCurrency = 0; }
    }

    public int GetCurrentCurrency()
    {
        return storedCurrency;
    }

    public int GetWorth()
    {
        return (worth = value);
    }
}
