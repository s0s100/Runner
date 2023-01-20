using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Control UI elements on the game scene and update time scale
// Also update responsible for displaying result score /coins
public class UIController : MonoBehaviour
{
    private const float TIME_BEFORE_LATE_GAME_PAUSE = 1.0f;
    private const int MAIN_MENU_SCENE_NUMBER = 0;
    private const int GAME_SCENE_NUMBER = 1;

    // Defeat fields
    [SerializeField]
    private TMP_Text scoreResultText;
    [SerializeField]
    private TMP_Text currentCoinsText;
    [SerializeField]
    private TMP_Text coinsAddedText;

    // Menu links
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

    // Score/coin controllers
    private ScoreController scoreController;
    private CoinController coinController;

    // Used to update coin/score values after defeat
    //private bool isFillingData = false;
    //private float curScoreDefeat = 0.0f;
    //private float curCoinsDefeat = 0.0f;
    //private float scoreSpeedDefeat = 10.0f; // How fast does the value changes
    //private float coinSpeedDefeat = 1.0f; // How fast does the value changes


    private void Start()
    {
        scoreController = FindObjectOfType<ScoreController>();
        coinController = FindObjectOfType<CoinController>();
        this.enabled = false;
    }

    private void Update()
    {
        TextUpdate();
    
        if (Input.anyKey)
        {
            InstaTextUpdate();
            this.enabled = false;
        }
    }

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
        coinController.StoreCoins();
        SceneManager.LoadScene(GAME_SCENE_NUMBER);
    }

    public void GameDefeatMenu()
    {
        defeatMenu.SetActive(true);
        gameMenu.SetActive(false);
        coinController.StoreCoins();
        StartCoroutine(LateGameStop());
    }

    private IEnumerator LateGameStop()
    {
        yield return new WaitForSeconds(TIME_BEFORE_LATE_GAME_PAUSE);
        Time.timeScale = 0.0f;
        // isFillingData = true;
        this.enabled = true;
    }

    public void MainMenu()
    {
        coinController.StoreCoins();
        SceneManager.LoadScene(MAIN_MENU_SCENE_NUMBER);
    }

    public void DisableStartGameText()
    {
        // startGameText.DeleteObject();
        startGameText.gameObject.SetActive(false);
    }

    public TMP_Text GetHealthText()
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

    private void TextUpdate()
    {
        int coinsBeforeAdding = coinController.GetTotalAmount() - coinController.GetCoinsAdded();
        currentCoinsText.text = coinsBeforeAdding.ToString();
        coinsAddedText.text = "+ " + coinController.GetCoinsAdded().ToString();

        scoreResultText.text = scoreController.GetScore().ToString();
    }

    private void InstaTextUpdate()
    {
        currentCoinsText.text = coinController.GetTotalAmount().ToString();
        coinsAddedText.text = "";

        scoreResultText.text = scoreController.GetScore().ToString();
    }
}