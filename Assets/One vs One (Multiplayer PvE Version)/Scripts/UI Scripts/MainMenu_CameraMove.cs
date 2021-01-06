using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_CameraMove : MonoBehaviour
{
    public float startCameraSpeed;
    public float maxCameraSpeed;
    public float timeToReachMaxSpeed;
    [SerializeField] [ReadOnly] float currentCameraSpeed;
    public Camera mainCamera;
    void Start()
    {
        currentCameraSpeed = startCameraSpeed;
    }

    void Update()
    {
        currentCameraSpeed = Mathf.Lerp(startCameraSpeed, maxCameraSpeed, Time.time / timeToReachMaxSpeed);
        mainCamera.transform.Translate(Vector3.forward * currentCameraSpeed * Time.deltaTime);
    }
}
