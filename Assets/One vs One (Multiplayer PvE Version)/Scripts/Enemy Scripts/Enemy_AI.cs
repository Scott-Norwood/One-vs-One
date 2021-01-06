using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //|| Enemy Variables
    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Header("Movement Speed")]
    public float movementSpeedMin;
    public float movementSpeedMax;
    [Space]
    [Header("Action Distance")]
    public int actionDistanceMin;
    public int actionDistanceMax;
    [Space]
    [Header("Jump Angle")]
    public bool canJump = false;
    public int jumpAngleMin;
    public int jumpAngleMax;
    [Space]
    [Header("Attack Values")]
    public int attackDamage;

    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //|| Private Variables
    ///|////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    float targetDistance;
    int initialAngle;
    int randomNumber;
    bool isJumping;
    bool isMoving;
    Transform target;
    Transform self;
    Transform startingPosition;
    Rigidbody selfRb;
    Enemy_Health enemy_Health;
    Animator animator;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        self = transform;
        selfRb = GetComponent<Rigidbody>();
        startingPosition = self.transform;
        isJumping = false;
        isMoving = true;
        randomNumber = Random.Range(actionDistanceMin, actionDistanceMax);
        initialAngle = Random.Range(jumpAngleMin, jumpAngleMax);
        enemy_Health = GetComponent<Enemy_Health>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target != null)
        {
            DistanceToTarget();
            SeekTarget();

            if ((int)targetDistance == randomNumber && canJump)
            {
                JumpToTarget();
                isMoving = false;
                isJumping = true;
                animator.Play("ZombieJump", 0);
            }
        }
    }

    void SeekTarget()
    {
        if (isJumping == false)
        {
            transform.Translate(Vector3.forward * (Random.Range(movementSpeedMin, movementSpeedMax) * Time.deltaTime));
        }
    }

    void DistanceToTarget()
    {
        targetDistance = Vector3.Distance(self.localPosition, target.position);
    }

    void JumpToTarget()
    {
        if (isMoving == true)
        {
            float gravity = Physics.gravity.magnitude;
            // Selected angle in radians
            float angle = initialAngle * Mathf.Deg2Rad;

            // Positions of this object and the target on the same plane
            Vector3 planarTarget = new Vector3(target.position.x, 0, target.position.z);
            Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

            // Planar distance between objects
            float distance = Vector3.Distance(planarTarget, planarPostion);

            // Distance along the y axis between objects
            float yOffset = transform.position.y - target.position.y;

            float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

            Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

            // Final calculation
            float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (target.position.x > transform.position.x ? 1 : -1);
            Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

            // Fire!
            selfRb.velocity = finalVelocity;
        }
    }

    void OnCollisionEnter(Collision other) // Quick and dirty for testing, move to Enemy_Health when done testing.
    {
        if (other.collider.tag == "LevelBounds")
        {
            enemy_Health.DecreaseHealth(100);
        }
    }
}
