using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinController : MonoBehaviour
{
    private static string COIN_STORAGE = "TotalCoinAmount";

    [SerializeField]
    private TMP_Text coinText;
    private int coinsAdded = 0;
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
        int curAmount = PlayerPrefs.GetInt(COIN_STORAGE);
        PlayerPrefs.SetInt(COIN_STORAGE, curAmount + coinsThisRound);
        coinsAdded = coinsThisRound;
        coinsThisRound = 0;
    }

    public int GetCoinAmount()
    {
        return coinsThisRound;
    }

    public int GetTotalAmount()
    {
        return PlayerPrefs.GetInt(COIN_STORAGE);
    }

    public int GetCoinsAdded()
    {
        return coinsAdded;
    }
}
