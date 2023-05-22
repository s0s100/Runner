using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TextCoinSetter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text adderText;

    private void Awake()
    {
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        TMP_Text textBox = GetComponent<TMP_Text>();
        textBox.text = CoinController.GetCurAmount().ToString();
    }

    public void MakeAdditionTextNotification(int coinAmount)
    {
        string output = "+" + coinAmount.ToString();
        adderText.text = output;
        Animator animator = adderText.GetComponent<Animator>();
        animator.SetTrigger("Addition");
    }

    // Substraction actually
    public void MakeRemovalTextNotification(int coinAmount)
    {
        string output = "-" + coinAmount.ToString();
        adderText.text = output;
        Animator animator = adderText.GetComponent<Animator>();
        animator.SetTrigger("Removal");
    }
}
