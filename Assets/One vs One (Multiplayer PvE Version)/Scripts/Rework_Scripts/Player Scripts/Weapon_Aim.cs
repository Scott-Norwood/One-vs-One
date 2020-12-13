using UnityEngine;

public class Weapon_Aim : MonoBehaviour
{
    public float minRotationAngle = -45f;
    public float maxRotationAngle = 45f;
    public float sensitivity = 1f;
    public float degreesPerSecondToSettle;
    public Vector3 assumedRotation;
    private float mouseAngle;

    private void Awake()
    {
        assumedRotation = transform.eulerAngles;
    }

    private void Start()
    {
        // Keeps cursor from going behinds the player which does not reverse the weapon movement
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Gets the distance the mouse moved this frame, multiplied by the sensitivity value. Multiply all of it by -1f to fix the inverted movement bug.
        float mouseVelocity = Input.GetAxis("Mouse Y") * 2f * sensitivity * -1f;
        mouseAngle = CalculateMovement(mouseVelocity);

        assumedRotation = new Vector3(mouseAngle, assumedRotation.y, assumedRotation.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(assumedRotation), degreesPerSecondToSettle * Time.deltaTime);

        if (assumedRotation.x > 360 + maxRotationAngle)
        {
            assumedRotation.x = 360 + maxRotationAngle;
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + mouseVelocity, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    private float CalculateMovement(float mouseVelocity)
    {
        // If the amount the mouse moved would force the euler angles to go into the negatives we want to do the proper math to avoid errors.
        // This is because unity is a silly little engine that doesn't allow me to freaking use negative values for euler angles.
        if (assumedRotation.x + mouseVelocity < 0)
        {
            // If the angle is going to go into the negatives, add it to 360 because eulers don't support negative values.
            mouseVelocity = 360 + assumedRotation.x + mouseVelocity;
        }
        else
        {
            // We want to change the clamp depending on the hemisphere the gun is currently pointing in.
            if (assumedRotation.x + mouseVelocity > 180f)
            {
                // If it is in the top hemisphere, we want to use the normal transform unless it is greater than -maxRotationAngle
                // This effectively clamps the value.
                mouseVelocity = Mathf.Max(assumedRotation.x + mouseVelocity, 360 + -maxRotationAngle);
            }
            else
            {
                // If it is in the bottom hemisphere, we want to use the normal transform unless it is greater than minRotationAngle
                // This effectively clamps the value.
                mouseVelocity = Mathf.Min(assumedRotation.x + mouseVelocity, minRotationAngle * -1);
            }
        }
        return mouseVelocity;

        // This also works but is harder to read. Gotta have everything commented out so we can figure out what the purpose of this stupid one liner is.
        /*return transform.eulerAngles.x + mouseVelocity < 0 ? mouseVelocity = 360 + transform.eulerAngles.x + mouseVelocity :
        transform.eulerAngles.x + mouseVelocity > 180f ?
        Mathf.Max(transform.eulerAngles.x + mouseVelocity, 360 + -maxRotationAngle) :
        Mathf.Min(transform.eulerAngles.x + mouseVelocity, minRotationAngle);*/
    }
}

