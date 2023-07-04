using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{
    // For the purpose of this example, these buttons are for functionality testing:
    //[SerializeField] 
    //private Button loadBannerButton;
    //[SerializeField] 
    //private Button showBannerButton;
    //[SerializeField] 
    //private Button hideBannerButton;
    [SerializeField] 
    private BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;
    [SerializeField] 
    private string androidAdUnitId = "Banner_Android";
    [SerializeField] 
    private string iosAdUnitId = "Banner_iOS";
     
    private string adUnitId = null;
    private bool showingAd = true;

    void Awake()
    {
        DefineCurUnitId();

        // Disable the button until an ad is ready to show:
        //showBannerButton.interactable = false;
        //hideBannerButton.interactable = false;

        // Set the banner position:
        Advertisement.Banner.SetPosition(bannerPosition);

        // Configure the Load Banner button to call the LoadBanner() method when clicked:
        //loadBannerButton.onClick.AddListener(LoadBanner);
        //loadBannerButton.interactable = true;

        StartCoroutine(PrepareBannerAd());
    }

    private IEnumerator PrepareBannerAd()
    {
        yield return null;
        LoadBanner();
    }

    private void DefineCurUnitId()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#elif UNITY_EDITOR
        adUnitId = iosAdUnitId; // For now
#endif
    }

    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(adUnitId, options);
    }

    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        if (showingAd)
        {
            ShowBannerAd();
        }
    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    // Implement a method to call when the Show Banner button is clicked:
    public void ShowBannerAd()
    {
        //Debug.Log("Show function was called!");

        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(adUnitId, options);
    }
    //public void ShowBannerAd(string whereAndWhy)
    //{
    //    Debug.Log("Showing banner at: " + whereAndWhy);

    //    // Show the loaded Banner Ad Unit:
    //    ShowBannerAd();
    //}

    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        //Debug.Log("Hide function was called!");

        showingAd = false;

        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    //public void HideBannerAd(string whereAndWhy)
    //{
    //    Debug.Log("Hiding banner at: " + whereAndWhy);

    //    // Hide the banner:
    //    HideBannerAd();
    //}

    private void OnLevelWasLoaded(int level)
    {
        // Debug.Log("Current level OnLevelWasLoaded: " + level);
        if (SceneController.MENU_SCENE_NUMBER == level)
        {
            //if (!Advertisement.Banner.isLoaded)
            //{
            //    Debug.Log("Banner is not loaded");
            //}
            
            ShowBannerAd();
        }
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }
}
