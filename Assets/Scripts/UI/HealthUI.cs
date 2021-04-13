using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerMovement playerRef;
    public Text healthText;

    private void Awake()
    {
        healthText = GetComponent<Text>();
    }

    private void Update()
    {
        healthText.text = "Heatlh: " + playerRef.health.ToString() + " / " + playerRef.maxHealth.ToString();
    }
}
