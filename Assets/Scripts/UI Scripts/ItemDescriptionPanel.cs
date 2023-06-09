using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionPanel : MonoBehaviour
{
    [SerializeField]
    private AudioClip purchaseSound;

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
    private PopUpMenuController popUpMenuController;

    private void Start()
    {
        popUpMenuController = FindObjectOfType<PopUpMenuController>();
        skinDisplayControl = FindObjectOfType<SkinDisplayControl>();
        textCoinSetter = FindObjectOfType<TextCoinSetter>();
        sceneController = FindObjectOfType<SceneController>();
    }

    // Why did I implement interface if I didn't use it? :\
    public void ShowUpgradeDescription(UpgradeData upgradeData)
    {
        if (upgradeData == null)
        {
            Debug.Log("Upgrade does not exist");
            return;
        }

        itemName.text = upgradeData.GetName();
        itemDescription.text = upgradeData.GetDescription();

        UpdateUpgradeDescription(upgradeData);
    }

    public void SetUpgradeBuyOnClick(UpgradeData upgradeData)
    {
        buyButton.interactable = true;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => SetBuyButtonOnClick(upgradeData));
    }

    public void SetBuyButtonOnClick(UpgradeData upgradeData)
    {
        Button acceptButton = popUpMenuController.ActivateShopPopUpMenu("Are you sure you want it?");
        acceptButton.onClick.AddListener(() => BuyUpgrade(upgradeData));
    }

    private void UpdateUpgradeDescription(UpgradeData upgradeData)
    {
        if (upgradeData.IsBought())
        {
            DisableBuyButton();
        }
        else
        {
            buyButtonCoinImage.enabled = true;
            itemPrice.text = upgradeData.GetPrice().ToString();

            if (upgradeData.CanBuy(CoinController.GetTotalAmount()))
            {
                SetUpgradeBuyOnClick(upgradeData);
            }
            else
            {
                buyButton.interactable = false;
            }
        }

        SetCurrentUpgrades(upgradeData.GetUpgradeStatus(),
                upgradeData.GetMaxUpgradeStatus());
    }

    private void BuyUpgrade(UpgradeData upgradeData)
    {
        CoinController.AddNewCoins(-upgradeData.GetPrice());
        textCoinSetter.MakeRemovalTextNotification(upgradeData.GetPrice());
        textCoinSetter.UpdateCoinText();

        int curCoins = CoinController.GetTotalAmount();
        upgradeData.Buy(curCoins);
        sceneController.SaveFile();

        UpdateUpgradeDescription(upgradeData);

        AudioController.instance.PlayButtonClick();
        AudioController.instance.PlayUIEffect(purchaseSound);
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
        buyButton.onClick.AddListener(() => SetBuyButtonOnClick(skinData));
    }

    private void SetBuyButtonOnClick(SkinData skinData)
    {
        Button acceptButton = popUpMenuController.ActivateShopPopUpMenu("Are you sure you want it?");
        acceptButton.onClick.AddListener(() => BuySkin(skinData));
    }

    private void BuySkin(SkinData skinData)
    {
        int curCoins = CoinController.GetTotalAmount();
        skinData.Buy(curCoins);
        sceneController.SaveFile();

        CoinController.AddNewCoins(-skinData.GetPrice());
        textCoinSetter.MakeRemovalTextNotification(skinData.GetPrice());
        textCoinSetter.UpdateCoinText();

        DisableBuyButton();
        SetCurrentUpgrades(1, 1);

        skinDisplayControl.ChooseShownSkins();

        AudioController.instance.PlayButtonClick();
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