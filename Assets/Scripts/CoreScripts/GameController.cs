using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    private bool isDefeated = false;

    [SerializeField]
    private GameObject gameMenu;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject defeatMenu;
    [SerializeField]
    private BlinkingText startGameText;
    [SerializeField]
    private PrefabHolder startPosition;

    private float moveSpeeed = 2.0f; // Should increase over time

    private const float TIME_BEFORE_LATE_GAME_PAUSE = 2.0f;
    private const int MAIN_MENU_SCENE_NUMBER = 0;
    private const int GAME_SCENE_NUMBER = 1;

    private PlayerMovement playerMovement;
    private CameraController cameraFollowPlayer;
    private LevelGenerator LevelGenerator;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        cameraFollowPlayer = FindObjectOfType<CameraController>();
        LevelGenerator = FindObjectOfType<LevelGenerator>();

        // This is the first generated object - Start location
        GameObject lastGeneratedPrefab = LevelGenerator.GetLastGeneratedPrefab();
        startPosition = lastGeneratedPrefab.GetComponent<PrefabHolder>();
    }
    
    void Update()
    {
        if (Input.anyKey)
        {
            StartGame();
            Debug.Log("?");
            this.enabled = false;
        }
    }
    
    public float GetGameSpeed() { return moveSpeeed; }

    public void PauseGame() 
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(GAME_SCENE_NUMBER);
        
    }

    public void GameDefeat()
    {
        isDefeated = true;
        defeatMenu.SetActive(true);
        gameMenu.SetActive(false);
        StartCoroutine(LateGameStop());
        playerMovement.enabled = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE_NUMBER);
    }

    private IEnumerator LateGameStop()
    {
        yield return new WaitForSeconds(TIME_BEFORE_LATE_GAME_PAUSE);
        Time.timeScale = 0.0f; 
    }

    private void StartGame()
    {
        playerMovement.EnablePlayerAnimations();
        playerMovement.enabled = true;
        cameraFollowPlayer.enabled = true;
        LevelGenerator.enabled = true;
        // startGameText.DeleteObject();
        startGameText.gameObject.SetActive(false);
        startPosition.LateDestroy();
    }

    public bool IsDefeated()
    {
        return isDefeated;
    }

}
