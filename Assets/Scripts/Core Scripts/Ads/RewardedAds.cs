using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //[SerializeField] 
    //private Button showAdButton;
    [SerializeField] 
    private string androidAdUnitId = "Rewarded_Android";
    [SerializeField] 
    private string iosAdUnitId = "Rewarded_iOS";

    private string adUnitId = null;

    private bool isLoaded = false;

    private static int rewardAmount = 20;

    public static int GetRewardAmount()
    {
        return rewardAmount;
    }

    void Awake()
    {
        // Debug.Log("Rewarded ads awake was called!");

        defineCurUnitId();

        StartCoroutine(PrepareRewardedAds());
        //Disable the button until the ad is ready to show:
        //showAdButton.interactable = false;
    }

    private IEnumerator PrepareRewardedAds()
    {
        yield return null;
        InitializeRewardAd();
    }

    public void InitializeRewardAd()
    {
        LoadAd();
        OnUnityAdsAdLoaded(adUnitId);
    }

    private void defineCurUnitId()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#elif UNITY_EDITOR
        adUnitId = iosAdUnitId; // For now
#endif
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        // Debug.Log("Loading Ad: " + adUnitId);
        Advertisement.Load(adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(this.adUnitId))
        {
            isLoaded = true;
            // Configure the button to call the ShowAd() method when clicked:
            //showAdButton.onClick.AddListener(this.ShowAd);
            // Enable the button for users to click:
            //showAdButton.interactable = true;
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        //showAdButton.interactable = false;
        // Then show the ad:
        if (isLoaded)
        {
            Advertisement.Show(adUnitId, this);
        }
    }

    public void EnableButton()
    {
        //showAdButton.interactable = true;
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(this.adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Received specified amount of coins");

            // Grant a reward.
            CoinController.AddNewCoins(rewardAmount);
            // Also update coin text in the main menu

            TextCoinSetter textCoinSetter = FindObjectOfType<TextCoinSetter>();
            textCoinSetter.UpdateCoinText();
            textCoinSetter.MakeAdditionTextNotification(rewardAmount);
        } else
        {
            Debug.Log("Unity Ads Rewarded Ad Is Not Completed");
        }

        EnableButton();
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
        //showAdButton.onClick.RemoveAllListeners();
    }
}
