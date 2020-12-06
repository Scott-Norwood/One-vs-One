using UnityEngine;

public class LockAndHideCursor : MonoBehaviour
{
    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
