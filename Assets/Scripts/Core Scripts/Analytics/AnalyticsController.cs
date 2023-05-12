using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine.Analytics;

public class AnalyticsController : MonoBehaviour
{
    public static AnalyticsController instance;

    public static AnalyticsController Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (ConsentCheckException e)
        {
            Debug.Log(e.ToString());
        }
    }

    // Pass event name as parameter as an option :|
    public void BossReached()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            AnalyticsService.Instance.CustomData("bossReached");
            AnalyticsService.Instance.Flush();
        }
    }

    public void BossKilled()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            AnalyticsService.Instance.CustomData("bossKilled");
            AnalyticsService.Instance.Flush();
        }
    }

    public void LevelLoaded()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            AnalyticsService.Instance.CustomData("levelLoaded");
            AnalyticsService.Instance.Flush();
        }
    }
}
