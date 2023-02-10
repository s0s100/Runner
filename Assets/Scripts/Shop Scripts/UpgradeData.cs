using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData", order = 3)]
public class UpgradeData : ScriptableObject, IBuyable
{
    [SerializeField]
    private int curUpgradeStatus;
    [SerializeField]
    private string upgradeName;
    [SerializeField]
    private List<int> upgradePrice;

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
