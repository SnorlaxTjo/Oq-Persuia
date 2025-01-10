using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    bool isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu(!isPaused);
            PauseGame(!isPaused);
        }
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        isPaused = pause;
    }

    public void ShowPauseMenu(bool show)
    {
        pauseMenu.SetActive(show);
    }
}
