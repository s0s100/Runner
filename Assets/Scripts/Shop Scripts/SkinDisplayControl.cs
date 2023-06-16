using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinDisplayControl : MonoBehaviour
{
    private static string LAST_SKIN_INDEX = "LastSkinIndex";

    private SceneController sceneController;
    private List<SkinData> skins;
    private int curSkin;

    [SerializeField]
    private Image skinImage;

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        ChooseShownSkins();
    }

    public void ChooseShownSkins()
    {
        SelectShownSkins();

        // For now this solution, after save selected skin ID
        int lastSkinIndex = GetLastSkinIndex();
        if (skins.Count > lastSkinIndex)
        {
            curSkin = lastSkinIndex;
            UpdageCurSkin();
        }
        else
        {
            throw new System.Exception("Empty skin list");
        }
    }

    // Shows skins
    private void SelectShownSkins()
    {
        skins = sceneController.GetPlayerData().GetBoughtSkins();
    }

    public void UpdageCurSkin()
    {
        // Also store cur index state
        PlayerPrefs.SetInt(LAST_SKIN_INDEX, curSkin);

        Animator anim = skinImage.GetComponent<Animator>();
        SkinData skin = skins[curSkin];
        anim.runtimeAnimatorController = skin.GetPreviewRuntimeAnimator();
    }

    public void ChooseLeftSkin()
    {
        if (curSkin == 0)
        {
            curSkin = skins.Count;
        }
        curSkin--;

        AudioController.instance.PlayButtonClickTwo();
        UpdageCurSkin();
    }

    public void ChooseRightSkin()
    {
        curSkin++;
        if (curSkin ==  skins.Count)
        {
            curSkin = 0;
        }

        AudioController.instance.PlayButtonClickTwo();
        UpdageCurSkin();
    }

    public static int GetLastSkinIndex()
    {
        return PlayerPrefs.GetInt(LAST_SKIN_INDEX);
    }
}
