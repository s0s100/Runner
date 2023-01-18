using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text coinText;
    private int coinsThisRound = 0;

    public void AddCoin()
    {
        coinsThisRound++;
        coinText.text = coinsThisRound.ToString();
    }

    public void AddCoins(int amount)
    {
        coinsThisRound += amount;
        coinText.text = coinsThisRound.ToString();
    }

    // TODO: Set coin number to zero and add it to global variable
    public void StoreCoins()
    {

    }

    public int GetCoinAmount()
    {
        return coinsThisRound;
    }
}
