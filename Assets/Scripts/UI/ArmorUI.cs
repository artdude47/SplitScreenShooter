using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUI : MonoBehaviour
{
    public PlayerMovement playerRef;
    public Text text;
    // Update is called once per frame
    void Update()
    {
        text.text = "Armor: " + playerRef.shield + " / " + playerRef.maxShield;
    }
}
