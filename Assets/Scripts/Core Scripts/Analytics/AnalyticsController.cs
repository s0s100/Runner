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

    AnalyticsController()
    {
        instance = this;
    }

    async void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        try
        {
            await UnityServices.InitializeAsync();
            
        }
        catch (ConsentCheckException e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void BossReached()
    {
        //if (UnityServices.State == 0)
        //{
        Debug.Log(UnityServices.State.ToString());
        string speedModifier = GameController.GetSpeedModifier().ToString();
        Debug.Log("Speed modifier: " + speedModifier);
        //AnalyticsResult analyticsResult = Analytics.CustomEvent("bossReached" + speedModifier);
        AnalyticsService.Instance.CustomData("bossReached");
        AnalyticsService.Instance.Flush();

        // Debug.Log("Boss reached with status: " + analyticsResult.ToString());
        //}
    }
}
