using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Main/Projectile")]
public class Weapon_Projectile_SO : ScriptableObject
{
    [Header("Projectile Properties")]
    public Rigidbody projectilePrefab;
    public int projectileVelocity;
    public float projectileDamage;
    public int projectileImpactForce;
    [Space]
    [Header("Explosive Properties")]
    public bool isExplosive;
    public int projectileExplosionRadius;
    [Space]
    [Header("Projectile Effects")]
    public AudioClip projectileAudioClip;
    public GameObject projectileMuzzleFlash;
    public GameObject projectileShellEffect;
}
