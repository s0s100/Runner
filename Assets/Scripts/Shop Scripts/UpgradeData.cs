using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeData : IBuyable
{
    [SerializeField]
    private int curUpgradeStatus;
    [SerializeField]
    private string upgradeName;
    [SerializeField]
    private List<int> upgradePrice;

    public UpgradeData(int curUpgradeStatus, string upgradeName, List<int> upgradePrice)
    {
        this.curUpgradeStatus = curUpgradeStatus;
        this.upgradeName = upgradeName;
        this.upgradePrice = upgradePrice;
    }

    public int Buy(int curMoney)
    {
        if (CanBuy(curMoney))
        {
            curMoney -= upgradePrice[curUpgradeStatus];
            curUpgradeStatus++;
        }

        return curMoney;
    }

    public bool CanBuy(int curMoney)
    {
        if (curMoney >= upgradePrice[curUpgradeStatus] && curUpgradeStatus < upgradePrice.Count)
        {
            return true;
        }

        return false;
    }

    public int GetPrice()
    {
        return upgradePrice[curUpgradeStatus];
    }

    public bool IsBought()
    {
        if (curUpgradeStatus < upgradePrice.Count)
        {
            return false;
        }

        return true;
    }
    

}
