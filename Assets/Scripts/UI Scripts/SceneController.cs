using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsMenu;

    private const int GAME_SCENE_NUMBER = 1;
    
    public void StartGameScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(GAME_SCENE_NUMBER);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }
}