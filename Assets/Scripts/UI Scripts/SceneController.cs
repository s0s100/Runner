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
    [SerializeField]
    private ShopController shopController;

    private const float LOAD_TIME = 1.0f;
    public const int MENU_SCENE_NUMBER = 0;
    public const int GAME_SCENE_NUMBER = 1;

    private void Update()
    {
        UpdateFile();
    }

    public void StartGameScene()
    {
        Time.timeScale = 1.0f;
        GameController.ResetSpeedModifier();
        ScoreController.ResetLastRoundScore();
        StartCoroutine(LateStartGame());

        AudioController.instance.PlayButtonClick();
    }

    private IEnumerator LateStartGame()
    {
        blackScreen.SetBool("IsInvisible", false);

        yield return new WaitForSeconds(LOAD_TIME);

        SceneManager.LoadScene(GAME_SCENE_NUMBER);

        BannerAds bannerAds = AdsInitializer.instance.gameObject.GetComponent<BannerAds>();
        bannerAds.HideBannerAd();

        AnalyticsController analyticsController = AnalyticsController.instance;
        if (analyticsController != null)
        {
            analyticsController.LevelLoaded();
        }
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

    public void ShowAdvertisements()
    {   
        PopUpMenuController popUpMenu = FindObjectOfType<PopUpMenuController>();
        string popUpText = "Would you like to watch advertisement to receive " 
            + RewardedAds.GetRewardAmount() + " coins?";
        popUpMenu.ActivateAdsPopUpMenu(popUpText);
        AudioController.instance.PlayButtonClick();
    }

    public void OpenShop()
    {
        shopController.gameObject.SetActive(true);
        AudioController.instance.ReducedMusicVolume();
        AudioController.instance.PlayButtonClick();

        BannerAds bannerAds = AdsInitializer.instance.gameObject.GetComponent<BannerAds>();
        bannerAds.HideBannerAd();
    }

    public void CloseShop()
    {
        shopController.gameObject.SetActive(false);
        AudioController.instance.MaxMusicVolume();
        AudioController.instance.PlayButtonClick();

        BannerAds bannerAds = AdsInitializer.instance.gameObject.GetComponent<BannerAds>();
        bannerAds.ShowBannerAd();
    }

    public void AddMoney()
    {
        int quantity = 1000;

        CoinController.AddNewCoins(quantity);

        TextCoinSetter coinSetter = FindObjectOfType<TextCoinSetter>();
        coinSetter.MakeAdditionTextNotification(quantity);
        coinSetter.UpdateCoinText();
    }
}