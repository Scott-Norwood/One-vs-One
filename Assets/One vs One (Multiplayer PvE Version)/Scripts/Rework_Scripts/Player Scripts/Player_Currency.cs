using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Currency : System_Currency_Core
{
    public override void IncreasePoints(int worth)
    {
        base.IncreasePoints(worth);
    }
    public override void DecreasePoints()
    {
        base.DecreasePoints();
    }
    public override int GetCurrentCurrency()
    {
        return base.GetCurrentCurrency();
    }
}
