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

    public void StartGameScene()
    {
        Time.timeScale = 1.0f;
        GameController.ResetSpeedModifier();
        ScoreController.ResetLastRoundScore();
        StartCoroutine(lateStartGame());

        BannerAds bannerAds = AdsInitializer.instance.gameObject.GetComponent<BannerAds>();
        bannerAds.HideBannerAd();
    }

    private IEnumerator lateStartGame()
    {
        blackScreen.SetBool("IsInvisible", false);

        yield return new WaitForSeconds(LOAD_TIME);

        SceneManager.LoadScene(GAME_SCENE_NUMBER);

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
        // RewardedAds rewardedAds = AdsInitializer.instance.gameObject.GetComponent<RewardedAds>();
        // rewardedAds.ShowAd();

        PopUpMenuController popUpMenu = FindObjectOfType<PopUpMenuController>();
        string popUpText = "Would you like to watch advertisement to receive " 
            + RewardedAds.GetRewardAmount() + " coins?";
        popUpMenu.ActivatePopUpMenu(popUpText);
    }

    public void OpenShop()
    {
        shopController.gameObject.SetActive(true);
    }
}