using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 4)]
public class WeaponData : ScriptableObject, IBuyable
{
    [SerializeField]
    private bool isOwned;
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private int weaponPrice;

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
