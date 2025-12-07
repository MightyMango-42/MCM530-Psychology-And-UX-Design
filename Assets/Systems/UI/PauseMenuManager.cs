using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    PlayerInputHandler inputHandler;
    [SerializeField] GameObject menu;

    private void Start()
    {
        inputHandler = PlayerInputHandler.Instance;
    }

    public void OnOpen()
    {
        menu.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public void OnClose()
    {
        GameManager.Instance.ResumeGame();
        menu.SetActive(false);
    }

    public void OnExitToMainMenu()
    {
        GameManager.Instance.LoadScene("Main Menu");
    }

    private void Update()
    {
        if (!GameManager.Instance.canOpenPauseMenu) return;

        if (inputHandler.pauseAction.WasReleasedThisFrame() && menu.activeSelf) OnClose();
        else if (inputHandler.pauseAction.WasReleasedThisFrame() && !menu.activeSelf) OnOpen();
    }
}
