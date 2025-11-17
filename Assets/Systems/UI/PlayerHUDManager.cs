using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] GameObject playerTimerObject;
    [SerializeField] GameObject playerSpeedObject;

    [Header("Popups")]
    [SerializeField] GameObject popupPanel;
    [SerializeField] float popupPanelDisplayTime;

    private TextMeshProUGUI playerTimerText;
    private TextMeshProUGUI playerSpeedText;

    private TextMeshProUGUI popupText;

    private float currentTime;
    private float playerSpeed;

    private void Awake()
    {
        playerTimerText = playerTimerObject.GetComponent<TextMeshProUGUI>();
        playerSpeedText = playerSpeedObject.GetComponent<TextMeshProUGUI>();
        popupText = popupPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        playerSpeedText.text = $"Speed: {Math.Round(playerSpeed, 2)}m/s";
    }

    public IEnumerator OpenPopupPanel(string text)
    {
        popupPanel.SetActive(true);
        popupText.text = text;

        yield return new WaitForSeconds(popupPanelDisplayTime);

        ClosePopupPanel();
    }

    private void ClosePopupPanel()
    {
        popupText.text = "";
        popupPanel.SetActive(false);
    }

    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = (time % 1) * 1000;

        playerTimerText.text = string.Format("{00:00}:{01:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public void SetPlayerSpeed(float speed)
    {
        playerSpeed = speed;
    }

    private void SetTimeText(string text)
    {
        playerTimerText.text = string.Format(text);
    }

    public void SetPopupText(string text)
    {
        popupText.text = text;
    }
}
