using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public PlayerMovement playerRef;
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        text.text = playerRef.equippedGun.currentAmmo.ToString() + " / " + playerRef.equippedGun.gunObject.maxAmmo.ToString();
    }
}
