using UnityEngine;

public class EmergencyLightRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.right * Time.deltaTime * 250f);
    }
}
