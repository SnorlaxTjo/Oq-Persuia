using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int sceneToLoad)
    {
        int scene = LoopBuildIndex(sceneToLoad);
        SceneManager.LoadScene(scene);
    }

    public void LoadNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        int scene = LoopBuildIndex(nextScene);
        SceneManager.LoadScene(scene);
    }

    public void ReloadScene()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Debug.Log("QUTI GAME");
        Application.Quit();
    }

    int LoopBuildIndex(int index)
    {
        if (index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Scene index out of range, loading scene 0");
            return 0;
        }
        else
        {
            return index;
        }
    }
}
