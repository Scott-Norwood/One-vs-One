using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.IO;
using System;


[Serializable]
public class TestSavePoints : MonoBehaviour
{
    [SerializeField] int _pointsToSave;
    [SerializeField] int _loadedSavePoints;

    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        _pointsToSave = GameManager.Instance.Points;
        StartCoroutine(SavePointsInterval());
    }

    void SavePoints()
    {
        MMSaveLoadManager.Save(_pointsToSave, "PlayerPoints" + ".txt", "One vs One (Multiplayer PvE Version)/");
    }

    IEnumerator SavePointsInterval()
    {
        yield return new WaitForSeconds(1f);
        SavePoints();
        Debug.Log("Saved.");
    }
}
