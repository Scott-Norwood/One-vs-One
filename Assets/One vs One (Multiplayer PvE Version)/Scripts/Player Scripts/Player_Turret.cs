using UnityEngine;

public class Player_Turret : MonoBehaviour
{
    public GameObject[] turrets;
    public Transform turretSpawnPoint;
    [SerializeField] [ReadOnly] int turretSlotNumber;
    [SerializeField] [ReadOnly] int curretSlotNumber;
    [SerializeField] [ReadOnly] GameObject currentTurret;

    void Start()
    {
        turretSlotNumber = 0;
    }

    public void SetTurretNumber(int slotnumber)
    {
        turretSlotNumber = slotnumber;
        curretSlotNumber = turretSlotNumber;
    }

    public void SpawnTurret()
    {
        if (turretSlotNumber > 0 && turretSlotNumber == curretSlotNumber)
        {
            if (turretSlotNumber == curretSlotNumber)
            {
                Destroy(currentTurret);
                currentTurret = Instantiate(turrets[turretSlotNumber], turretSpawnPoint.position, turretSpawnPoint.rotation);
            }
        }
        //Debug.Log(" Current slot #: " + turretSlotNumber + " Current Turret: " + currentTurret.name);
    }
}
