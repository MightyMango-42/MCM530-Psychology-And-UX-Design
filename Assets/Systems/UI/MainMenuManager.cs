using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject levelSelectPanel;

    private void Start()
    {
        GameManager.Instance.UnlockCursor();
    }

    public void OnNewGameClicked()
    {
        GameManager.Instance.LoadScene("Tutorial");
    }

    public void OnLevelSelectClicked()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void OnReturnButtonClicked()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
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
