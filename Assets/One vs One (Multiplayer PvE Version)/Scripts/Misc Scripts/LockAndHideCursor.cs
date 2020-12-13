using UnityEngine;

public class LockAndHideCursor : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
