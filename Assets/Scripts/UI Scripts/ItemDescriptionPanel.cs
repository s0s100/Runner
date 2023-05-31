using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionPanel : MonoBehaviour
{
    [SerializeField]
    private Image[] upgradeLevels;
    [SerializeField]
    private TMP_Text itemName;
    [SerializeField]
    private TMP_Text itemDescription;
    [SerializeField]
    private TMP_Text itemPrice;
    [SerializeField]
    private Image buyButtonCoinImage;
    [SerializeField]
    private Button buyButton;

    [SerializeField]
    private Sprite notUpgradedImage;
    [SerializeField]
    private Sprite upgradedImage;
    
    private SkinDisplayControl skinDisplayControl;
    private SceneController sceneController;
    private TextCoinSetter textCoinSetter;

    private void Start()
    {
        skinDisplayControl = FindObjectOfType<SkinDisplayControl>();
        textCoinSetter = FindObjectOfType<TextCoinSetter>();
        sceneController = FindObjectOfType<SceneController>();
    }

    public void ShowSkinDescription(SkinData skinData)
    {
        if (skinData == null)
        {
            Debug.Log("Skin does not exist");
            return;
        }

        itemName.text = skinData.GetName();
        itemDescription.text = skinData.GetDescription();
        
        // Check if not bought
        if (skinData.IsBought())
        {
            DisableBuyButton();
            SetCurrentUpgrades(1, 1);
        } else
        {
            buyButtonCoinImage.enabled = true;
            itemPrice.text = skinData.GetPrice().ToString();

            if (skinData.CanBuy(CoinController.GetTotalAmount()))
            {
                SetSkinBuyOnClick(skinData);
            } else
            {
                buyButton.interactable = false;
            }

            SetCurrentUpgrades(0, 1);
        }
    }

    private void SetSkinBuyOnClick(SkinData skinData)
    {
        buyButton.interactable = true;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => BuySkin(skinData));
        //buyButton.onClick.AddListener(TestButton);
        Debug.Log("In Button functionality is set!");
    }

    private void BuySkin(SkinData skinData)
    {
        Debug.Log("I was called 1 !");

        int curCoins = CoinController.GetTotalAmount();
        skinData.Buy(curCoins);

        sceneController.SaveFile();
        CoinController.AddNewCoins(-skinData.GetPrice());
        textCoinSetter.MakeRemovalTextNotification(skinData.GetPrice());
        textCoinSetter.UpdateCoinText();

        DisableBuyButton();
        SetCurrentUpgrades(1, 1);

        skinDisplayControl.ChooseShownSkins();
    }

    private void DisableBuyButton()
    {
        buyButton.onClick.RemoveAllListeners();
        buyButtonCoinImage.enabled = false;
        itemPrice.text = "Sold";
        buyButton.interactable = false;
    }

    private void SetCurrentUpgrades(int curUpgrageLevel, int maxUpgradeLevel)
    {
        for (int i = 0; i < upgradeLevels.Length; i++)
        {
            if (i >= maxUpgradeLevel)
            {
                upgradeLevels[i].enabled = false;
            } else
            {
                upgradeLevels[i].enabled = true;
            }     

            if (i < curUpgrageLevel)
            {
                upgradeLevels[i].sprite = upgradedImage;
            } else
            {
                upgradeLevels[i].sprite = notUpgradedImage;
            }
        }
    }
}