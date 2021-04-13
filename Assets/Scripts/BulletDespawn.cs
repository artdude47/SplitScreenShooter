using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespawn : MonoBehaviour
{
    public float lifeInSeconds = 3f;
    // Start is called before the first frame update
    void Start()
    {
        lifeInSeconds += Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lifeInSeconds) Destroy(this.gameObject);
    }
}
