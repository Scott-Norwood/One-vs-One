using UnityEngine;

public class Enemy_Currency : MonoBehaviour
{
    public int enemyValue = 1;

    public void HeadShot(int headshotValue)
    {
        enemyValue = headshotValue;
        //Debug.Log(enemyValue);
    }
    public void BodyShot(int bodyshotValue)
    {
        enemyValue = bodyshotValue;
        //Debug.Log(enemyValue);
    }
}