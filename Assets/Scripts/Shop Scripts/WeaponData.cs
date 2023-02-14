using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData : IBuyable
{
    [SerializeField]
    private bool isOwned;
    [SerializeField]
    private string pathToWeapon;
    [SerializeField]
    private int weaponPrice;

    public WeaponData(bool isOwned, string pathToWeapon, int weaponPrice)
    {
        this.isOwned = isOwned;
        this.pathToWeapon = pathToWeapon;
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

    public Weapon GetWeapon()
    {
        Weapon skinPreview = Resources.Load<Weapon>(pathToWeapon);
        return skinPreview;
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
