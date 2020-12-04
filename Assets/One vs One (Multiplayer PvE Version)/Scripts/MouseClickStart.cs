using UnityEngine;
using UnityEngine.Playables;

public class MouseClickStart : MonoBehaviour
{

    void OnMouseDown()
    {
        PlayableDirector playableDirector = FindObjectOfType<PlayableDirector>();
        playableDirector.Play();
    }
}
