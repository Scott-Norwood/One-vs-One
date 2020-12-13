using UnityEngine;
using UnityEngine.Playables;

public class UserLevelStart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayableDirector playableDirector = FindObjectOfType<PlayableDirector>();
            playableDirector.Play();
        }
    }
}
