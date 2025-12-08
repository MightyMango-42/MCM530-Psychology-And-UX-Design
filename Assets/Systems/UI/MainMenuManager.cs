using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SettingsManager settingsManager;
    [Space]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject levelSelectPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject settingsPanel;
    [Space]
    [SerializeField] GameObject selectedMenuButtonObject;
    [SerializeField] GameObject selectedLevelButtonObject;
    [SerializeField] GameObject selectedCreditsButtonObject;
    [SerializeField] GameObject selectedSettingsButtonObject;

    private void Start()
    {
        GameManager.Instance.UnlockCursor();
        EventSystem.current.SetSelectedGameObject(selectedMenuButtonObject);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
    }

    public void OnNewGameClicked()
    {
        GameManager.Instance.LoadScene("Tutorial");
    }

    public void OnLevelSelectClicked()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(selectedLevelButtonObject);
    }

    public void OnSettingsClicked()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        settingsManager.OnOpen();

        EventSystem.current.SetSelectedGameObject(selectedSettingsButtonObject);
    }

    public void OnCreditsClicked()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(selectedCreditsButtonObject);
    }

    public void OnReturnButtonClicked()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(selectedMenuButtonObject);
    }

    public void OnCreditsReturnButtonClicked()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(selectedMenuButtonObject);
    }

    public void OnSettingsReturnButtonClicked()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(selectedMenuButtonObject);
    }

    public void LoadLevel(string level)
    {
        GameManager.Instance.LoadScene(level);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
