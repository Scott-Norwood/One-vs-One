using UnityEngine;
using Cinemachine;

public class CMCameraSwap : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera1;
    public CinemachineVirtualCamera virtualCamera2;
    public CinemachineVirtualCamera virtualCamera3;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            virtualCamera1.m_Priority = 1;
            virtualCamera2.m_Priority = 0;
            virtualCamera3.m_Priority = 0;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            virtualCamera1.m_Priority = 0;
            virtualCamera2.m_Priority = 1;
            virtualCamera3.m_Priority = 0;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            virtualCamera1.m_Priority = 0;
            virtualCamera2.m_Priority = 0;
            virtualCamera3.m_Priority = 1;
        }
    }
}
