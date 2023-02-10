using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData : IBuyable
{
    [SerializeField]
    private bool isOwned;
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private int weaponPrice;

    public WeaponData(bool isOwned, Weapon weapon, int weaponPrice)
    {
        this.isOwned = isOwned;
        this.weapon = weapon;
        this.weaponPrice = weaponPrice;
    }

    public int Buy(int curMoney)
    {
        if (CanBuy(curMoney))
        {
            curMoney -= weaponPrice;
            isOwned = true;
        }

        return curMoney;
    }

    public bool CanBuy(int curMoney)
    {
        if (curMoney >= weaponPrice && !isOwned)
        {
            return true;
        }

        return false;
    }

    public int GetPrice()
    {
        return weaponPrice;
    }

    public bool IsBought()
    {
        return isOwned;
    }
}
