using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class SkinData :  IBuyable
{
    // Keyword before skin animation
    private static string ADDITIONAL_ANIMATOR_KEYWORD = "Animator";
    private static string ADDITIONAL_PREVIEW_KEYWORD = "Preview";

    [SerializeField]
    private bool isOwned;
    [SerializeField]
    private string skinName;
    [SerializeField]
    private int skinPrice;
    [SerializeField]
    private string pathToAnimators;

    public SkinData(bool isOwned, string skinName, int skinPrice, string pathToAnimators)
    {
        this.isOwned = isOwned;
        this.skinName = skinName;
        this.skinPrice = skinPrice;
        this.pathToAnimators = pathToAnimators;
    }

    public AnimatorController GetPreview()
    {
        string resultPreviewPath = Path.Combine(pathToAnimators, ADDITIONAL_PREVIEW_KEYWORD);
        AnimatorController skinPreview = Resources.Load<AnimatorController>(resultPreviewPath);

        return skinPreview;
        //return skinPreview;
    }

    public AnimatorController GetAnimator()
    {
        string resultSkinName = skinName + ADDITIONAL_ANIMATOR_KEYWORD;
        string resultPreviewPath = Path.Combine(pathToAnimators, resultSkinName);
        AnimatorController skinPreview = Resources.Load<AnimatorController>(resultPreviewPath);

        return skinPreview;
    }

    public int Buy(int curMoney)
    {
        if (CanBuy(curMoney))
        {
            curMoney -= skinPrice;
            isOwned = true;
        }

        return curMoney;
    }

    public bool CanBuy(int curMoney)
    {
        if (curMoney >= skinPrice && !isOwned)
        {
            return true;
        }

        return false;
    }

    public int GetPrice()
    {
        return skinPrice;
    }

    public bool IsBought()
    {
        return isOwned;
    }
}
