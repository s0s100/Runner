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

    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void HidePanel()
    {
        canvas.enabled = false;
    }

    public void UpdateItemDescription(SkinData skinData)
    {
        if (skinData == null)
        {
            Debug.Log("Skin does not exist");
            return;
        }

        canvas.enabled = true;

        itemName.text = skinData.GetName();
        itemDescription.text = skinData.GetDescription();
        
        // Check if not bought
        if (skinData.IsBought())
        {
            buyButtonCoinImage.enabled = false;
            itemPrice.text = "Sold";
            buyButton.interactable = false;
            SetCurrentUpgrades(1, 1);
        } else
        {
            buyButtonCoinImage.enabled = true;
            itemPrice.text = skinData.GetPrice().ToString();
            buyButton.interactable = true;
            SetCurrentUpgrades(0, 1);
        }
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
