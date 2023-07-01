using System.IO;
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
    [SerializeField]
    private string description;
    [SerializeField]
    private Color attackColor;

    public SkinData(bool isOwned, string skinName, int skinPrice, string pathToAnimators)
    {
        this.isOwned = isOwned;
        this.skinName = skinName;
        this.skinPrice = skinPrice;
        this.pathToAnimators = pathToAnimators;
    }

    public RuntimeAnimatorController GetPreviewRuntimeAnimator()
    {
        string resultPreviewName = skinName + ADDITIONAL_PREVIEW_KEYWORD;
        string resultPreviewPath = Path.Combine(pathToAnimators, resultPreviewName);
        // Animator skinPreview = Resources.Load<Animator>(resultPreviewPath);
        RuntimeAnimatorController runtimeAnimator = Resources.Load<RuntimeAnimatorController>(resultPreviewPath);

        //return skinPreview.runtimeAnimatorController;
        return runtimeAnimator;
    }

    public RuntimeAnimatorController GetRuntimeAnimator()
    {
        string resultSkinName = skinName + ADDITIONAL_ANIMATOR_KEYWORD;
        string resultPreviewPath = Path.Combine(pathToAnimators, resultSkinName);
        // Animator skinPreview = Resources.Load<Animator>(resultPreviewPath);
        RuntimeAnimatorController runtimeAnimator = Resources.Load<RuntimeAnimatorController>(resultPreviewPath);

        return runtimeAnimator;
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

    public Color GetColor()
    {
        return attackColor;
    }

    public string GetName()
    {
        return skinName;
    }

    public string GetDescription()
    {
        return description;
    }
}
