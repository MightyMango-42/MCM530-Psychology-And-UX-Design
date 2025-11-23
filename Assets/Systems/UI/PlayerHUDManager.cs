using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] GameObject playerTimerObject;
    [SerializeField] GameObject playerSpeedObject;
    private TextMeshProUGUI playerTimerText;
    private TextMeshProUGUI playerSpeedText;

    [Header("Popups")]
    [SerializeField] GameObject popupPanel;
    [SerializeField] float popupPanelDisplayTime;
    private TextMeshProUGUI popupText;

    [Header("LevelStats")]
    [SerializeField] GameObject levelCompletionPanel;
    [SerializeField] GameObject currentRunTimeObject;
    [SerializeField] GameObject currentRunScoreObject;
    private TextMeshProUGUI currentRunTimeText;
    private TextMeshProUGUI currentRunScoreText;

    private float currentTime;
    private float playerSpeed;

    private void Awake()
    {
        playerTimerText = playerTimerObject.GetComponent<TextMeshProUGUI>();
        playerSpeedText = playerSpeedObject.GetComponent<TextMeshProUGUI>();

        popupText = popupPanel.GetComponentInChildren<TextMeshProUGUI>();

        currentRunTimeText = currentRunTimeObject.GetComponent<TextMeshProUGUI>();
        currentRunScoreText = currentRunScoreObject.GetComponent<TextMeshProUGUI>();
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

    public void OpenLevelCompletionPanel(int score, string collectables)
    {
        GameManager.Instance.canOpenPauseMenu = false;

        currentRunTimeText.text = $"Run Time: {playerTimerText.text}";
        currentRunScoreText.text = $"Score: {score}\nCollectables Found: {collectables}";

        GameManager.Instance.UnlockCursor();
        levelCompletionPanel.SetActive(true);
    }

    public void OnContinueClicked()
    {
        GameManager.Instance.LoadNextScene();
        GameManager.Instance.canOpenPauseMenu = true;
    }

    public void OnQuitClicked()
    {
        GameManager.Instance.LoadScene("Main Menu");
    }

    public void SetTime(float time)
    {
        playerTimerText.text = UpdateTimer(time);
    }

    private string UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = (time % 1) * 1000;

        return string.Format("{00:00}:{01:00}:{2:00}", minutes, seconds, milliseconds);
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
