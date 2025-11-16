using System;
using TMPro;
using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] GameObject playerTimerObject;
    [SerializeField] GameObject playerSpeedObject;

    private TextMeshProUGUI playerTimerText;
    private TextMeshProUGUI playerSpeedText;

    private float currentTime;
    private float playerSpeed;

    private void Awake()
    {
        playerTimerText = playerTimerObject.GetComponent<TextMeshProUGUI>();
        playerSpeedText = playerSpeedObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        //playerTimerText.text = $"Time: {currentTime}";
        playerSpeedText.text = $"Speed: {Math.Round(playerSpeed, 2)}m/s";
    }

    public void SetPlayerSpeed(float speed)
    {
        playerSpeed = speed;
    }
}
