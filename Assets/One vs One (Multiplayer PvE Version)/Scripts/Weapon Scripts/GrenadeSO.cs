using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GrenadeType
{
    FragGrenade,
    FireGrenade,
    ImplosionGrenade,
    AntigravityGrenade
}

[CreateAssetMenu(fileName = "Grenade", menuName = "Silent Project/GrenadeSO", order = 0)]
public class GrenadeSO : ScriptableObject
{
    public GrenadeType grenadeType;
    public ParticleSystem explosionEffect;
    public ParticleSystem groundEffect;
    public AudioClip explosionSound;
    public int grenadeDamage;
    public int grenadeForce;
    public int explosionRadius;
    public int upwardExplosionForce;
    public float fuseTime;
    public float impulseForce;
    public float implosionPulseTime;
}
