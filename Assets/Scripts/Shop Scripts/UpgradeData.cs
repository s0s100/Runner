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
    private string upgradeDescription;
    [SerializeField]
    private List<int> upgradePrices;

    public UpgradeData(int curUpgradeStatus, string upgradeName, List<int> upgradePrice)
    {
        this.curUpgradeStatus = curUpgradeStatus;
        this.upgradeName = upgradeName;
        this.upgradePrices = upgradePrice;
    }

    public int Buy(int curMoney)
    {
        if (CanBuy(curMoney))
        {
            curMoney -= upgradePrices[curUpgradeStatus];
            curUpgradeStatus++;
        }

        return curMoney;
    }

    public bool CanBuy(int curMoney)
    {
        if (curMoney >= upgradePrices[curUpgradeStatus] && curUpgradeStatus < upgradePrices.Count)
        {
            return true;
        }

        return false;
    }

    public int GetPrice()
    {
        return upgradePrices[curUpgradeStatus];
    }

    public bool IsBought()
    {
        if (curUpgradeStatus < upgradePrices.Count)
        {
            return false;
        }

        return true;
    }

    public string GetName()
    {
        return upgradeName;
    }

    public int GetUpgradeStatus()
    {
        return curUpgradeStatus;
    }

    public int GetMaxUpgradeStatus()
    {
        return upgradePrices.Count;
    }
    
    public string GetDescription()
    {
        return upgradeDescription;
    }
}
