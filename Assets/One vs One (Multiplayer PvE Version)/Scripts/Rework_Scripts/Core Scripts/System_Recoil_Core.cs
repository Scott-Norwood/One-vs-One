// Copyright (c) 2020 xtilly5000
// Licensed under the Creative Commons Attribution-ShareAlike 4.0 International license.
// Share — copy and redistribute the material in any medium or format.
// Adapt — remix, transform, and build upon the material for any purpose, even commercially.

using UnityEngine;

namespace xtilly5000.Prototypes.RecoilManager
{
    #region RecoilManager Class
    /// <summary>
    /// Recoil manager manages recoil for weapons. It changes the rotation of the gun as well as the location of the gun.
    /// </summary>
    public class System_Recoil_Core : MonoBehaviour
    {
        #region Variables
        // The maximum angle we can rotate the gun.
        [Header("Angle Settings")]
        [Range(0, 80)]
        public float maxAngle = 35f;

        // The minimum angle we can rotate the gun.
        [Range(0, -80)]
        public float minAngle = -35f;

        // How many degrees per second do we want to recover.
        [Header("Recoil Settings")]
        public float climbRecoverySpeed = 20f;

        // The number of degrees to kick back when recoiling.
        public Vector2 climbAmount = new Vector2(2, 4);

        // The amount of distance to kick back.
        public Vector2 kickbackAmount = new Vector2(0.4f, 0.8f);

        // The speed at which the gun recovers from kickback.
        public float kickbackRecoverySpeed = 20f;

        // Multiplies the sensitivity of the mouse.
        [Header("Other")]
        public float sensitivity = 1f;

        // Do we want to lock the cursor on start?
        public bool hideCursor = true;

        // How many degrees of rotation do we need to rotate in order to fully recover.
        private float amountToRecover;

        // The position that the gun started at.
        private Vector3 startingPosition;

        // Used by SmoothDamp to keep track of movement velocity last frame.
        private Vector3 kickbackVelocity;
        #endregion

        #region Start() Method
        private void Start()
        {
            // If we want to hide the cursor on start then hide it.
            if (hideCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Cache the position that the gun started in so we can recover from kickback.
            startingPosition = transform.localPosition;
        }
        #endregion

        #region Update() Method
        private void Update()
        {
            // Get the amount of distance the mouse moved this frame, and multiply it by the sensitivity value.
            float mouseVelocity = Input.GetAxis("Mouse Y") * 2f * sensitivity * -1f;

            // We want to rotate the gun that amount, and remove that distance from the amountToRecover if we need to.
            transform.Rotate(mouseVelocity, 0, 0);
            amountToRecover -= mouseVelocity < 0 ? 0f : mouseVelocity;

            // If we still have to recover from previous recoil, then do it.
            if (amountToRecover > 0)
            {
                // Calculate the amount of distance we need to rotate according to the recovery speed.
                float change = climbRecoverySpeed * Time.deltaTime;

                // Rotate that distance and remove that amount from the amountToRecover.
                transform.Rotate(change, 0f, 0f);
                amountToRecover = amountToRecover - change;
            }
            else
            {
                // If somehow the amountToRecover goes below zero due to calculation errors we don't want to mess up aiming as a result.
                amountToRecover = 0f;
            }

            // If our angle is smaller than the minimum angle, we want to clamp it.
            if (transform.eulerAngles.x > -minAngle && transform.eulerAngles.x - 360 < -maxAngle - 30)
            {
                transform.eulerAngles = new Vector3(-minAngle, transform.eulerAngles.y, 0);
            }

            // If our angle is larger than the maximum angle, we want to clamp it.
            if (transform.eulerAngles.x > -minAngle + 30 && transform.eulerAngles.x < 360 + -maxAngle)
            {
                transform.eulerAngles = new Vector3(-maxAngle, transform.eulerAngles.y, 0);
            }
        }
        #endregion

        #region LateUpdate() Method
        private void LateUpdate()
        {
            // After we modify the guns current position, we want to lerp it back to its original position.
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, startingPosition, ref kickbackVelocity, 1 / (Vector3.Distance(transform.position, startingPosition) * kickbackRecoverySpeed));
        }
        #endregion

        #region Recoil() Method
        /// <summary>
        /// Recoils the gun properly.
        /// </summary>
        public void Recoil()
        {
            // Randomize the amount of degrees we want the barrel to climb.
            float _climbAmount = Random.Range(climbAmount.x, climbAmount.y);

            // Record the amount of recoil we added to the gun to the amountToRecover.
            amountToRecover += _climbAmount;

            // Rotate the specified amount of degrees upward.
            transform.Rotate(-_climbAmount, 0f, 0f);

            // Move the gun backwards locally.
            transform.position += transform.TransformDirection(Vector3.back) * (Random.Range(kickbackAmount.x, kickbackAmount.y) / 3);
        }
        #endregion
    }
    #endregion
}
