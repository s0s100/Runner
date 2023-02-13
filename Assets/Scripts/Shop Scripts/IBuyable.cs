using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuyable
{
    int Buy(int curMoney);
    bool CanBuy(int curMoney);
    int GetPrice();
    bool IsBought();
}
