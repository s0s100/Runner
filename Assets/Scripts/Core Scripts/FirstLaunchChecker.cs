using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLaunchChecker : MonoBehaviour
{
    private const string IS_FIRST_LAUNCH_PREF = "FirstLaunch";

    [SerializeField]
    private GameObject tutorialWindow;

    private void Awake()
    {
        bool isFirst = IsFirstLaunch();
        if (isFirst)
        {
            ShowTutorial();
        }
    }

    private bool IsFirstLaunch()
    {
        bool isFirstLaunch = PlayerPrefs.GetInt(IS_FIRST_LAUNCH_PREF, 1) == 1;

        if (isFirstLaunch)
        {
            PlayerPrefs.SetInt(IS_FIRST_LAUNCH_PREF, 0);
            PlayerPrefs.Save();
        }

        return isFirstLaunch;
    }

    public void ResetFirstLaunch()
    {
        PlayerPrefs.DeleteKey(IS_FIRST_LAUNCH_PREF);
    }

    public void ShowTutorial()
    {
        Instantiate(tutorialWindow);
    }
}
