using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButtonController : MonoBehaviour
{
    private Button musicButton;
    private Image buttonImage;

    [SerializeField]
    private Sprite enabledMusic;
    [SerializeField]
    private Sprite disabledMusic;

    void Start()
    {
        musicButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        DefineButtonStatus();
    }

    private void DefineButtonStatus()
    {
        bool status = AudioController.instance.GetMusicSoundStatus();

        if (status)
        {
            SetDisableSound();
        } else
        {
            SetEnableSound();
        }
    }

    private void SetEnableSound()
    {
        buttonImage.sprite = disabledMusic;
        musicButton.onClick.AddListener(OnClickEnableSound);
    }

    private void OnClickEnableSound()
    {
        musicButton.onClick.RemoveAllListeners();

        AudioController.instance.SetMusicSounds(true);
        AudioController.instance.PlayButtonClick();
        SetDisableSound();
    }

    private void SetDisableSound()
    {
        buttonImage.sprite = enabledMusic;
        musicButton.onClick.AddListener(OnClickDisableSound);
    }

    private void OnClickDisableSound()
    {
        musicButton.onClick.RemoveAllListeners();

        AudioController.instance.SetMusicSounds(false);
        AudioController.instance.PlayButtonClick();
        SetEnableSound();
    }
}
