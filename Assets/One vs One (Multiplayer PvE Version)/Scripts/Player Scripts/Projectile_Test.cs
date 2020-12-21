using UnityEngine;
using System.Collections;

public class Projectile_Test : MonoBehaviour
{
    public float speed;
    public Transform trace;
    public GameObject explosion;
    public AudioClip explosionAudio;

    public int penetration = 20;
    public int damage = 10;

    public LayerMask layerMask; //make sure we aren't in this layer
    private Vector3 previousPos;
    private Vector3 thisPos;
    private Vector3 stepDirection;
    private float stepSize;

    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {

        rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, 10);
        Shoot();
    }

    void FixedUpdate()
    {
        RaycastHit hitInfo;
        thisPos = transform.position;
        stepDirection = rb.velocity.normalized;
        stepSize = (thisPos - previousPos).magnitude;
        if (stepSize > 0.1)
        {
            if (Physics.Raycast(previousPos, stepDirection, out hitInfo, stepSize, layerMask))
            {
                Destruct(hitInfo.point, hitInfo.normal, hitInfo.transform.root);
            }
            else
            {
                previousPos = thisPos;
            }
        }
    }

    void Destruct(Vector3 point, Vector3 normal, Transform target)
    {
        Quaternion hitNormal = Quaternion.Euler(normal);
        //GameObject boom = Instantiate(explosion,point,hitNormal);
        Instantiate(explosion, point, hitNormal);
        AudioSource.PlayClipAtPoint(explosionAudio, transform.position, 2f);
        Destroy(gameObject);
    }

    void Shoot()
    {
        previousPos = transform.position;
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }
}

