using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class SkinData :  IBuyable
{
    [SerializeField]
    private AnimatorController skinPreview;
    [SerializeField]
    private AnimatorController skin;

    [SerializeField]
    private bool isOwned;
    [SerializeField]
    private string skinName;
    [SerializeField]
    private int skinPrice;

    public SkinData(AnimatorController skinPreview, AnimatorController skin, bool isOwned, string skinName, int skinPrice)
    {
        this.skinPreview = skinPreview;
        this.skin = skin;
        this.isOwned = isOwned;
        this.skinName = skinName;
        this.skinPrice = skinPrice;
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
