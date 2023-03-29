using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private StoragePlayerData playerData;
    [SerializeField]
    private Animator blackScreen;

    private const float LOAD_TIME = 1.0f;
    private const int GAME_SCENE_NUMBER = 1;

    private void Awake()
    {
        // UpdateFile();
    }

    public void StartGameScene()
    {
        Time.timeScale = 1.0f;
        GameController.ResetSpeedModifier();
        ScoreController.ResetLastRoundScore();
        StartCoroutine(lateStartGame());
    }

    private IEnumerator lateStartGame()
    {
        blackScreen.SetBool("IsInvisible", false);

        yield return new WaitForSeconds(LOAD_TIME);

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