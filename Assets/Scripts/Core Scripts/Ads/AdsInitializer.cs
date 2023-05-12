using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    // private readonly string gameToken = "f1bf3e7a-6c1d-43f0-8cb1-8772445002ee";

    public static AdsInitializer instance;
    public static AdsInitializer Instance { get { return instance; } }

    [SerializeField]
    private string androidGameId = "5234309";
    [SerializeField]
    private string iosGameId = "5234308";
    private string gameId = null;

    [Header("Unity Test Mode")]
    [SerializeField]
    private bool testMode = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        defineCurGameID();

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Debug.Log("Initializing ads!");
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    private void defineCurGameID()
    {
#if UNITY_IOS
        gameId = iosGameId;
#elif UNITY_ANDROID
        gameId = androidGameId;
#elif UNITY_EDITOR
        gameId = iosGameId; // For now
#endif
    }

    public void OnInitializationComplete()
    {
        // Debug.Log("OnInitializationComplete");
        // throw new System.NotImplementedException();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("OnInitializationFailed, error string: " + error.ToString());
        Debug.Log("OnInitializationFailed, message: " + message);
        // throw new System.NotImplementedException();
    }
}
