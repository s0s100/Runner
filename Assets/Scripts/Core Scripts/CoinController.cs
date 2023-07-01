using UnityEngine;
using TMPro;

public class CoinController : MonoBehaviour
{
    private static string COIN_STORAGE = "TotalCoinAmount";
    private static string LAST_LEVEL_COIN_STORAGE = "LastLevelCoins";

    [SerializeField]
    private TMP_Text coinText;
    private int coinsAdded = 0;
    private int coinsThisRound = 0;

    private void Start()
    {
        coinsThisRound = GetLastLevelCoins();
        // Debug.Log("Coins[" + coinsThisRound + "]  TotalCoins[" + GetTotalAmount() + "]");
    }

    public static void AddNewCoins(int amount)
    {
        int curAmount = PlayerPrefs.GetInt(COIN_STORAGE, 0);
        curAmount += amount;
        PlayerPrefs.SetInt(COIN_STORAGE, curAmount);
    }

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
        int curAmount = PlayerPrefs.GetInt(COIN_STORAGE, 0);
        PlayerPrefs.SetInt(COIN_STORAGE, curAmount + coinsThisRound);
        coinsAdded = coinsThisRound;
        coinsThisRound = 0;
    }

    public int GetCoinAmount()
    {
        return coinsThisRound;
    }

    public static int GetTotalAmount()
    {
        return PlayerPrefs.GetInt(COIN_STORAGE, 0);
    }

    public int GetCoinsAdded()
    {
        return coinsAdded;
    }

    public int GetLastLevelCoins()
    {
        int result = PlayerPrefs.GetInt(LAST_LEVEL_COIN_STORAGE, 0);
        PlayerPrefs.SetInt(LAST_LEVEL_COIN_STORAGE, 0);

        return result;
    }

    public void SaveForNextLevelCoins()
    {
        PlayerPrefs.SetInt(LAST_LEVEL_COIN_STORAGE, coinsThisRound);
    }
}
