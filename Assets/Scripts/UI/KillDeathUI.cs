using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillDeathUI : MonoBehaviour
{
    public PlayerMovement playerRef;
    public Text killText;
    public Text deathText;

    // Update is called once per frame
    void Update()
    {
        killText.text = playerRef.kills.ToString();
        deathText.text = playerRef.deaths.ToString();
    }
}
