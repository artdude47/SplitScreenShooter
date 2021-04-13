using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunSO gunObject;
    public int currentAmmo;
    public float nextFire;
    public Transform gunEnd;
    public ParticleSystem muzzleFlash;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = gunObject.maxAmmo;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
