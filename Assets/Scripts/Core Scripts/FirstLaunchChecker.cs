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
            Debug.Log("It is a first launch");
            ShowTutorial();
        } else
        {

            Debug.Log("It is not a first launch");
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
        Debug.Log("Resetting first launch");
        PlayerPrefs.DeleteKey(IS_FIRST_LAUNCH_PREF);
    }

    private void ShowTutorial()
    {
        Instantiate(tutorialWindow);
    }
}
