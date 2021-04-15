using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelShot : MonoBehaviour
{
    public AudioClip explosion;
    public ParticleSystem explosionSystem;

    public void Shot()
    {
        explosionSystem.Play();
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
