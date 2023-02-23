using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to simplify development of the game
public static class DevelopmentData
{
    // Paths to players prefs
    private static string SHOULD_UPDATE_PREF = "Should Update";
    private static string FIRST_GENERATED_BLOCK_PREF = "Is Last block";
    private static string IS_NEXT_BLOCK_COIN_PREF = "Is next block coin";
    
    // 0 is false, 1 is true, int player pref is used
    public static void SetShouldUpdate(bool isUpdating)
    {
        int result = 0;
        if (isUpdating){ result = 1; }

        PlayerPrefs.SetInt(SHOULD_UPDATE_PREF, result);
    }

    // 0 is false, 1 is true, int player pref is used
    public static void SetFirstGeneratedBlock(bool IsLastBlock)
    {
        int result = 0;
        if (IsLastBlock) { result = 1; }

        PlayerPrefs.SetInt(FIRST_GENERATED_BLOCK_PREF, result);
    }
    
    // 0 is false, 1 is true, int player pref is used
    public static void SetIsCoinType(bool isCoin)
    {
        int result = 0;
        if (isCoin) { result = 1; }

        PlayerPrefs.SetInt(IS_NEXT_BLOCK_COIN_PREF, result);
    }

    public static bool GetShouldUpdate()
    {
        int shouldUpdate = PlayerPrefs.GetInt(SHOULD_UPDATE_PREF);
        if (shouldUpdate == 1)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public static bool GetNextGeneratedBlock()
    {
        int isLastBlock = PlayerPrefs.GetInt(FIRST_GENERATED_BLOCK_PREF);
        if (isLastBlock == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool GetIsCoinType()
    {
        int isNextCoin = PlayerPrefs.GetInt(IS_NEXT_BLOCK_COIN_PREF);
        if (isNextCoin == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
