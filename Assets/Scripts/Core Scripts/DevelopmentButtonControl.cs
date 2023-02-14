using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevelopmentButtonControl : MonoBehaviour
{
    [SerializeField]
    private Button enableUpgradesButton;
    [SerializeField]
    private Button nextCoinLocButton;
    [SerializeField]
    private Button nextLastLocButton;

    private void Start()
    {
        ModifyButtonsImages();
    }

    private void ModifyButtonsImages() {
        bool shouldUpgrade = DevelopmentData.GetShouldUpdate();
        SetButtonColor(shouldUpgrade, enableUpgradesButton);

        bool isNextCoin = DevelopmentData.GetIsCoinType();
        SetButtonColor(isNextCoin, nextCoinLocButton);

        bool isNextLastLoc = DevelopmentData.GetNextGeneratedBlock();
        SetButtonColor(isNextLastLoc, nextLastLocButton);
    }

    // Green if active, red otherwise
    private void SetButtonColor(bool isActive, Button button)
    {
        Image image = button.GetComponent<Image>();
        if (isActive)
        {
            image.color = Color.green;
        }
        else
        {
            image.color = Color.red;
        }
    }

    public void EnableUpgradesButtonClick()
    {
        bool curStatus = !DevelopmentData.GetShouldUpdate();
        DevelopmentData.SetShouldUpdate(curStatus);
        SetButtonColor(curStatus, enableUpgradesButton);
    }

    public void NextCoinLocButtonClick()
    {
        bool curStatus = !DevelopmentData.GetIsCoinType();
        DevelopmentData.SetIsCoinType(curStatus);
        SetButtonColor(curStatus, nextCoinLocButton);
    }

    public void NextLastLocButtonClick()
    {
        bool curStatus = !DevelopmentData.GetNextGeneratedBlock();
        DevelopmentData.SetFirstGeneratedBlock(curStatus);
        SetButtonColor(curStatus, nextLastLocButton);
    }
}
