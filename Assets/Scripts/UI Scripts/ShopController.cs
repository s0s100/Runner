using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private ItemDescriptionPanel itemDescription;

    private SceneController sceneController;
    private List<SkinData> skins;
    private int curSkin;

    [SerializeField]
    private Image skinImage;

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        skins = sceneController.GetPlayerData().GetSkinList();

        if (skins.Count > 0)
        {
            curSkin = 0;
            UpdageCurSkin();
            DisplayData();
        }
        else
        {
            throw new System.Exception("Empty skin list");
        }
    }

    public void UpdageCurSkin()
    {
        Animator anim = skinImage.GetComponent<Animator>();
        anim.runtimeAnimatorController = skins[curSkin].GetPreview() as RuntimeAnimatorController;
    }

    public void ChooseLeftSkin()
    {
        if (curSkin == 0)
        {
            curSkin = skins.Count;
        }
        curSkin--;
        UpdageCurSkin();
        DisplayData();
    }

    public void ChooseRightSkin()
    {
        curSkin++;
        if (curSkin == skins.Count)
        {
            curSkin = 0;
        }
        UpdageCurSkin();
        DisplayData();
    }

    public void DisplayData()
    {
        itemDescription.ShowSkinDescription(skins[curSkin]);
    }
}