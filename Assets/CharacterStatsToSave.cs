using UnityEngine;
using MoreMountains.CorgiEngine;

public class CharacterStatsToSave : MonoBehaviour
{
    Health health;
    int deaths;

    void Start()
    {
        health = GetComponent<Health>();
        deaths = ES3.Load("Player Deaths", deaths);
        print("We've Died: " + deaths + " times.");
    }
    void Update() // If we die, iterate deaths, save the value
    {
        if (health.CurrentHealth <= 0)
        {
            deaths++;
            ES3.Save("Player Deaths", deaths);
        }
        else { return; }
    }
}
