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
    private const float BLACK_SCREEN_TRANSITION_TIME = 1.5f;
    private const float TIME_BEFORE_LATE_GAME_PAUSE = 1.0f;
    private const int MAIN_MENU_SCENE_NUMBER = 0;
    private const int GAME_SCENE_NUMBER = 1;

    // Defeat fields
    [SerializeField]
    private TMP_Text scoreResultText;
    [SerializeField]
    private TMP_Text lastResultText;
    [SerializeField]
    private TMP_Text currentCoinsText;
    [SerializeField]
    private TMP_Text coinsAddedText;

    // Menu links
    [SerializeField]
    private GameObject gameMenu;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject defeatMenu;
    [SerializeField]
    private BlinkingText startGameText;
    [SerializeField]
    private Button pauseMenuButton;

    [SerializeField]
    private Animator blackScreen;

    [SerializeField]
    private AudioClip defeatSound;

    // Score/coin/player controllers
    private ScoreController scoreController;
    private CoinController coinController;
    private PlayerController playerController;

    private void Start()
    {
        scoreController = FindObjectOfType<ScoreController>();
        coinController = FindObjectOfType<CoinController>();
        playerController = FindObjectOfType<PlayerController>();
        this.enabled = false;
    }

    private void Update()
    {    
        if (Input.anyKey)
        {
            InstaTextUpdate();
            this.enabled = false;
        }
    }

    public void PauseGame()
    {
        //gameMenu.SetActive(false);
        pauseMenuButton.gameObject.SetActive(false);
        pauseMenu.SetActive(true);
        playerController.SetControllable(false);
        Time.timeScale = 0.0f;

        GameObject bannerAdsObject = AdsInitializer.instance.gameObject;
        if (bannerAdsObject != null)
        {
            bannerAdsObject.GetComponent<BannerAds>().ShowBannerAd();
        }

        AudioController.instance.ReducedMusicVolume();
        AudioController.instance.PlayButtonClick();
    }

    public void ResumeGame()
    {
        //gameMenu.SetActive(true);
        pauseMenuButton.gameObject.SetActive(true);
        playerController.SetControllable(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;

        BannerAds bannerAds = AdsInitializer.instance.gameObject.GetComponent<BannerAds>();
        bannerAds.HideBannerAd();

        AudioController.instance.MaxMusicVolume();
        AudioController.instance.PlayButtonClick();
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        coinController.StoreCoins();
        GameController.ResetSpeedModifier();
        ScoreController.ResetLastRoundScore();
        SceneManager.LoadScene(GAME_SCENE_NUMBER);

        AudioController.instance.PlayButtonClick();

        BannerAds bannerAds = AdsInitializer.instance.gameObject.GetComponent<BannerAds>();
        bannerAds.HideBannerAd();
    }

    public void GameDefeatMenu()
    {
        defeatMenu.SetActive(true);
        gameMenu.SetActive(false);
        coinController.StoreCoins();
        StartCoroutine(LateGameStop());

        AudioController.instance.PauseMusic();
        AudioController.instance.PlayEffect(defeatSound, Camera.main.transform.position);

        BannerAds bannerAds = AdsInitializer.instance.gameObject.GetComponent<BannerAds>();
        bannerAds.ShowBannerAd();
    }

    private IEnumerator LateGameStop()
    {
        yield return new WaitForSeconds(TIME_BEFORE_LATE_GAME_PAUSE);
        TextUpdate();
        Time.timeScale = 0.0f;
        // isFillingData = true;
        this.enabled = true;
    }

    public void MainMenu()
    {
        coinController.StoreCoins();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(MAIN_MENU_SCENE_NUMBER);

        AudioController.instance.PlayButtonClick();
    }

    public void StartNextLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(GAME_SCENE_NUMBER);
    }

    public void DisableStartGameText()
    {
        // startGameText.DeleteObject();
        startGameText.gameObject.SetActive(false);
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
        int coinsBeforeAdding = CoinController.GetTotalAmount() - coinController.GetCoinsAdded();
        currentCoinsText.text = coinsBeforeAdding.ToString();
        coinsAddedText.text = "+ " + coinController.GetCoinsAdded().ToString();

        scoreResultText.text = scoreController.GetScore().ToString();

        string output;
        if (scoreController.UpdateMaxScore())
        {
            output = "New record!";
        } else
        {
            output = "Best record: " + ScoreController.GetMaxScore();
            lastResultText.color = new Color(255,100,100);
        }

        lastResultText.text = output;
    }

    private void InstaTextUpdate()
    {
        currentCoinsText.text = CoinController.GetTotalAmount().ToString();
        coinsAddedText.text = "";

        scoreResultText.text = scoreController.GetScore().ToString();
    }

    public float IsBlackScreenInvisible(bool isInvisible)
    {
        blackScreen.SetBool("IsInvisible", isInvisible);
        return BLACK_SCREEN_TRANSITION_TIME;
    }
}