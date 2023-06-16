using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private ItemDescriptionPanel itemDescription;

    private SceneController sceneController;
    private List<SkinData> skins;
    private int curSkin;

    [SerializeField]
    private Image skinImage;

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        skins = sceneController.GetPlayerData().GetSkinList();

        if (skins.Count > 0)
        {
            curSkin = 0;
            UpdageCurSkin();
            DisplaySkinData();
        }
        else
        {
            throw new System.Exception("Empty skin list");
        }
    }

    public void UpdageCurSkin()
    {
        Animator anim = skinImage.GetComponent<Animator>();
        anim.runtimeAnimatorController = skins[curSkin].GetPreviewRuntimeAnimator();
    }

    public void ChooseLeftSkin()
    {
        if (curSkin == 0)
        {
            curSkin = skins.Count;
        }
        curSkin--;
        UpdageCurSkin();
        DisplaySkinData();

        AudioController.instance.PlayButtonClickTwo();
    }

    public void ChooseRightSkin()
    {
        curSkin++;
        if (curSkin == skins.Count)
        {
            curSkin = 0;
        }
        UpdageCurSkin();
        DisplaySkinData();

        AudioController.instance.PlayButtonClickTwo();
    }

    public void ClickSkinUpdate()
    {
        DisplaySkinData();

        AudioController.instance.PlayButtonClickTwo();
    }

    private void DisplaySkinData()
    {
        itemDescription.ShowSkinDescription(skins[curSkin]);
    }

    public void DisplayJumpData()
    {
        UpgradeData upgrade = sceneController.GetPlayerData().GetUpgradeData("Double Jump");
        itemDescription.ShowUpgradeDescription(upgrade);

        AudioController.instance.PlayButtonClickTwo();
    }

    public void DisplayHealthData()
    {
        UpgradeData upgrade = sceneController.GetPlayerData().GetUpgradeData("MaxHP");
        itemDescription.ShowUpgradeDescription(upgrade);

        AudioController.instance.PlayButtonClickTwo();
    }

    public void DisplayAttackSpeedData()
    {
        UpgradeData upgrade = sceneController.GetPlayerData().GetUpgradeData("Attack Speed");
        itemDescription.ShowUpgradeDescription(upgrade);

        AudioController.instance.PlayButtonClickTwo();
    }
}