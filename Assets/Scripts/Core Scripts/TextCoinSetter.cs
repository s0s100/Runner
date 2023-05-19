using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TextCoinSetter : MonoBehaviour
{
    private void Awake()
    {
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        TMP_Text textBox = GetComponent<TMP_Text>();
        textBox.text = CoinController.GetCurAmount().ToString();
    }
}
