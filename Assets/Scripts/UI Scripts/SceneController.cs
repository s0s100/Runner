using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private StoragePlayerData playerData;

    private const int GAME_SCENE_NUMBER = 1;

    private void Awake()
    {
        // Updage file before staring the scene
        if (DevelopmentData.GetShouldUpdate())
        {
            UpdateFile();
        }
    }

    public void StartGameScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(GAME_SCENE_NUMBER);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void SaveFile()
    {
        StoragePlayerManager.Save(playerData);;
    }
    
    public void UpdateFile()
    {
        StoragePlayerManager.Load(playerData);
    }

    public StoragePlayerData GetPlayerData()
    {
        return playerData;
    }
}