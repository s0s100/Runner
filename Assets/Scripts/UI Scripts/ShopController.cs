using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    private SceneController sceneController;
    private ItemDescriptionPanel itemDescription;
    private List<SkinData> skins;
    private int curSkin;

    [SerializeField]
    private Image skinImage;

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        itemDescription = GetComponentInChildren<ItemDescriptionPanel>();
        skins = sceneController.GetPlayerData().GetSkinList();

        if (skins.Count > 0)
        {
            curSkin = 0;
            UpdageCurSkin();
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
        SkinTouched();
    }

    public void ChooseRightSkin()
    {
        curSkin++;
        if (curSkin == skins.Count)
        {
            curSkin = 0;
        }
        UpdageCurSkin();
        SkinTouched();
    }

    public void SkinTouched()
    {
        itemDescription.UpdateItemDescription(skins[curSkin]);
    }
}