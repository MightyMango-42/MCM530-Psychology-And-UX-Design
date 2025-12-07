using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string nextSceneToLoad;
    public bool GamePaused {get; private set;}
    public bool canOpenPauseMenu = true;

    [Header("Level Scoring Parameters")]
    public int maxLevelScore = 1000;
    public int bracketBoundarySeparationInSeconds = 30;

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
        UnlockCursor();
    }
    public void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = 1f;
        LockCursor();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneToLoad)
    {
        LockCursor();
        ResumeGame();

        SceneManager.LoadScene(sceneToLoad);
    }
    public void LoadNextScene()
    {
        LoadScene(nextSceneToLoad);
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
