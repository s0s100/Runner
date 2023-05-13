using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField]
    private string androidAdUnitId = "Interstitial_Android";
    [SerializeField]
    private string iosAdUnitId = "Interstitial_iOS";

    private string adUnitId = null;

    private void Awake()
    {
        defineCurUnitId();
    }

    private void defineCurUnitId()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#elif UNITY_EDITOR
        gameId = iosAdUnitId; // For now
#endif
    }

    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + adUnitId);
        Advertisement.Load(adUnitId, this);
    }

    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + adUnitId);
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("OnUnityAdsAdLoaded, placement ID: " + placementId);
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("OnUnityAdsFailedToLoad, placement ID: " + placementId);
        Debug.Log("OnUnityAdsFailedToLoad, error: " + error.ToString());
        Debug.Log("OnUnityAdsFailedToLoad, message: " + message);
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("OnUnityAdsShowClick, placement ID: " + placementId);
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("OnUnityAdsShowComplete, placement ID: " + placementId);
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("OnUnityAdsShowFailure, placement ID: " + placementId);
        Debug.Log("OnUnityAdsShowFailure, error string: " + error.ToString());
        Debug.Log("OnUnityAdsShowFailure, message: " + message);
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("OnUnityAdsShowStart, placement ID: " + placementId);
        // throw new System.NotImplementedException();
    }
}
