﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCameraRotation : MonoBehaviour
{
    public float rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.unscaledDeltaTime, 0);
    }
}
