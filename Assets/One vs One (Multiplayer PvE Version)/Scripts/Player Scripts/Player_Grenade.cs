using System.Collections;
using UnityEngine;

public class Player_Grenade : MonoBehaviour
{
    public Rigidbody[] grenade;
    int grenadeSlotNumber;
    public Transform grenadeSpawnPoint;
    public int power;
    public Transform grenadeTargetDistance;
    public float grenadeTargetMoveSpeed = 2f;
    Vector3 grenadeTargetDistanceStartPos;
    [SerializeField] [ReadOnly] float targetDistance;
    Vector3 originalScale;

    void Start() { grenadeTargetDistanceStartPos = grenadeTargetDistance.position; grenadeSlotNumber = 0; originalScale = grenadeTargetDistance.localScale; }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            grenadeTargetDistance.gameObject.SetActive(true);
            DistanceToTarget();
            if (targetDistance >= 6) { return; }
            else { StartCoroutine(ChargeToToss()); }
        }
        else
        {
            grenadeTargetDistance.gameObject.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Space)) { SpawnGrenade(); grenadeTargetDistance.position = grenadeTargetDistanceStartPos; }
    }

    IEnumerator ChargeToToss()
    {
        grenadeTargetDistance.transform.localScale = originalScale * grenade[grenadeSlotNumber].gameObject.GetComponent<GrenadeExplosion>().radius;
        grenadeTargetDistance.transform.localScale = new Vector3(grenadeTargetDistance.transform.localScale.x, originalScale.y, grenadeTargetDistance.transform.localScale.z);
        grenadeTargetDistance.transform.Translate(Vector3.right * grenadeTargetMoveSpeed * Time.deltaTime);
        yield return new WaitForSecondsRealtime(.25f);
    }
    public void SetGrenadeNumber(int slotnumber)
    {
        grenadeSlotNumber = slotnumber;
    }

    void SpawnGrenade()
    {
        Rigidbody grenadeInstance;
        int i = grenadeSlotNumber;
        grenadeInstance = Instantiate(grenade[i], grenadeSpawnPoint.position, grenadeSpawnPoint.rotation) as Rigidbody;
        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = power * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(grenadeTargetDistance.position.x, 0, grenadeTargetDistance.position.z);
        Vector3 planarPostion = new Vector3(grenadeInstance.position.x, 0, grenadeInstance.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);

        // Distance along the y axis between objects
        float yOffset = grenadeInstance.position.y - grenadeTargetDistance.position.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Final calculation
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) *
                                    (grenadeTargetDistance.position.x > grenadeInstance.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        // Fire!
        grenadeInstance.velocity = finalVelocity;

    }
    void DistanceToTarget() { targetDistance = Vector3.Distance(grenadeTargetDistanceStartPos, grenadeTargetDistance.position); }
}