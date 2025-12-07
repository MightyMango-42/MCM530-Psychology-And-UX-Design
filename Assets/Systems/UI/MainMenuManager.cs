using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject levelSelectPanel;
    [Space]
    [SerializeField] GameObject selectedMenuButtonObject;
    [SerializeField] GameObject selectedLevelButtonObject;

    private void Start()
    {
        GameManager.Instance.UnlockCursor();
        EventSystem.current.SetSelectedGameObject(selectedMenuButtonObject);
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

    public void OnReturnButtonClicked()
    {
        levelSelectPanel.SetActive(false);
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
