using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMenuController : MonoBehaviour
{
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private TMP_Text popUpText;

    private Canvas popUpCanvas;

    private void Start()
    {
        popUpCanvas = gameObject.GetComponent<Canvas>();

        // Add required button functionality
        cancelButton.onClick.AddListener(OnCancelClick);
    }

    private void OnCancelClick()
    {
        popUpCanvas.enabled = false;
        acceptButton.onClick.RemoveAllListeners();

        AudioController.instance.PlayButtonClick();
        AudioController.instance.MaxMusicVolume();
    }

    public void ActivateAdsPopUpMenu(string menuText)
    {
        SetAdsAcceptButton();
        popUpText.text = menuText;
        popUpCanvas.enabled = true;
        AudioController.instance.ReducedMusicVolume();
    }

    // Returns reference to the buy button
    public Button ActivateShopPopUpMenu(string menuText)
    {
        popUpText.text = menuText;
        popUpCanvas.enabled = true;
        acceptButton.onClick.AddListener(SetShopAcceptButton);
        AudioController.instance.ReducedMusicVolume();

        return acceptButton;
    }

    // Sets basic functionality to the button
    public void SetShopAcceptButton()
    {
        popUpCanvas.enabled = false;
        acceptButton.onClick.RemoveAllListeners();
    }

    private void SetAdsAcceptButton()
    {
        acceptButton.onClick.AddListener(OnAcceptAdsClick);
    }

    private void OnAcceptAdsClick()
    {
        RewardedAds rewardedAds = AdsInitializer.instance.gameObject.GetComponent<RewardedAds>();
        rewardedAds.ShowAd();

        popUpCanvas.enabled = false;
        acceptButton.onClick.RemoveAllListeners();

        AudioController.instance.PlayButtonClick();
        AudioController.instance.PauseMusic();
    }
}