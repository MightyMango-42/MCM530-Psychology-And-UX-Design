using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject menu;

    public void OnOpen()
    {
        menu.SetActive(true);
        GameManager.Instance.PauseGame();
        GameManager.Instance.UnlockCursor();
    }

    public void OnClose()
    {
        menu.SetActive(false);
        GameManager.Instance.ResumeGame();
        GameManager.Instance.LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menu.activeSelf) OnClose();
        else if (Input.GetKeyDown(KeyCode.Escape) && !menu.activeSelf) OnOpen();
    }
}
