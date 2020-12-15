using UnityEngine;

public class Weapon_Projectile_Hit : MonoBehaviour
{
    //public int impactForce;
    Enemy_Health enemy_Health;
    Enemy_Currency enemy_Currency;
    Player_Currency player_Currency;
    public Weapon_Projectile_SO weapon_Projectile_SO;
    void Awake()
    {
        player_Currency = FindObjectOfType<Player_Currency>();

    }
    void OnCollisionEnter(Collision other)
    {
        enemy_Health = other.gameObject.GetComponent<Enemy_Health>();
        enemy_Currency = other.gameObject.GetComponent<Enemy_Currency>();


        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<Enemy_Health>() != null)
        {
            enemy_Health.DecreaseHealth((int)weapon_Projectile_SO.projectileDamage);
            player_Currency.IncreasePoints(enemy_Currency.GetWorth());
            other.rigidbody.AddForce(transform.position, ForceMode.Impulse);
            Debug.Log(other.gameObject.name);
            Debug.Log("Enemy Worth: " + enemy_Currency.GetWorth());
        }

        Destroy(gameObject);
    }
}
