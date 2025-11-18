using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool GamePaused {get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PauseGame()
    {
        GamePaused = true;
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        UnlockCursor();
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadScene(string sceneToLoad)
    {
        LockCursor();
        ResumeGame();

        if (sceneToLoad == "MainMenu") LoadMainMenu();

        SceneManager.LoadScene(sceneToLoad);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
