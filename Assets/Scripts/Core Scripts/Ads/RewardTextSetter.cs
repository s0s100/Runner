using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RewardTextSetter : MonoBehaviour
{
    private void Awake()
    {
        TMP_Text textBox = GetComponent<TMP_Text>();
        textBox.text = RewardedAds.GetRewardAmount().ToString();
    }
}
