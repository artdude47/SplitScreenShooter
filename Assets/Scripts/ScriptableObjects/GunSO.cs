using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunSO : ScriptableObject
{
    public string gunName;
    public int maxAmmo;
    public float baseDamage;
    public float headMultiplier;
    public float fireRate;
    public float weaponRange;
    public float reloadTime;
    public GameObject bulletHole;
    public AudioClip gunShot;
    public AudioClip emptySound;

    public float recoil;
    public float aimSpeed;
    public Vector3 adsLocation;
    public Vector3 holdLocation;
    public AnimationClip reloadClip;
}
