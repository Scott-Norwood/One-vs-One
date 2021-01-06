using UnityEngine;
public class Weapon_LaserSight : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject lrStartpoint;
    public GameObject lrEndpoint;

    void Update()
    {
        lineRenderer.SetPosition(0, lrStartpoint.transform.position);
        lineRenderer.SetPosition(1, lrEndpoint.transform.position);
    }
}
