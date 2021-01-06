using UnityEngine;

public class HealthToLightingChange : MonoBehaviour
{
    public Light[] lights;
    Player_Health player_Health;
    [SerializeField] Color color1;
    [SerializeField] Color color2;
    void Start()
    {
        player_Health = FindObjectOfType<Player_Health>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.Lerp(color2, color1, Mathf.Clamp(player_Health.GetHealthPercentage(), 0, 1));
            if (player_Health.GetHealthPercentage() <= .30f)
            {
                lights[i].intensity = Mathf.PingPong(Time.time * 5, 6);
            }
        }

    }
}
