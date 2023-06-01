using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
        anim.runtimeAnimatorController = skins[curSkin].GetPreview() as RuntimeAnimatorController;
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
    }

    public void DisplaySkinData()
    {
        itemDescription.ShowSkinDescription(skins[curSkin]);
    }

    public void DisplayJumpData()
    {
        UpgradeData upgrade = sceneController.GetPlayerData().GetUpgradeData("Double Jump");
        itemDescription.ShowUpgradeDescription(upgrade);
    }

    public void DisplayHealthData()
    {
        UpgradeData upgrade = sceneController.GetPlayerData().GetUpgradeData("MaxHP");
        itemDescription.ShowUpgradeDescription(upgrade);
    }

    public void DisplayAttackSpeedData()
    {
        UpgradeData upgrade = sceneController.GetPlayerData().GetUpgradeData("Attack Speed");
        itemDescription.ShowUpgradeDescription(upgrade);
    }
}