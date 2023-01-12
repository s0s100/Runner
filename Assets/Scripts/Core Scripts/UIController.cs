using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Control UI elements on the game scene and update time scale
public class UIController : MonoBehaviour
{
    private const float TIME_BEFORE_LATE_GAME_PAUSE = 2.0f;
    private const int MAIN_MENU_SCENE_NUMBER = 0;
    private const int GAME_SCENE_NUMBER = 1;

    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private GameObject gameMenu;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject defeatMenu;
    [SerializeField]
    private BlinkingText startGameText;

    public void PauseGame()
    {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(GAME_SCENE_NUMBER);
    }

    public void GameDefeatMenu()
    {
        defeatMenu.SetActive(true);
        gameMenu.SetActive(false);
        StartCoroutine(LateGameStop());
    }

    private IEnumerator LateGameStop()
    {
        yield return new WaitForSeconds(TIME_BEFORE_LATE_GAME_PAUSE);
        Time.timeScale = 0.0f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE_NUMBER);
    }

    public void DisableStartGameText()
    {
        // startGameText.DeleteObject();
        startGameText.gameObject.SetActive(false);
    }

    public TMP_Text getHealthText()
    {
        return healthText;
    }

    // Used to define if any GUI is below touch position
    public bool ShouldDiscardSwipe(Vector2 touchPos)
    {
        PointerEventData touch = new PointerEventData(EventSystem.current)
        {
            position = touchPos
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(touch, raycastResults);

        // Debug.Log(raycastResults.Count > 0);
        return (raycastResults.Count > 0);
    }
}
