using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class GameMachineControl : MonoBehaviour
{
    private SceneController sceneController;
    private List<SkinData> skins;
    private int curSkin;

    [SerializeField]
    private Image skinImage;

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        skins = sceneController.GetPlayerData().GetSkinList();
        
        // For now this solution, after save selected skin ID
        if (skins.Count > 0)
        {
            curSkin = 0;
            UpdageCurSkin();
        } else
        {
            curSkin = -1;
            throw new System.Exception("Empty skin list");
        }
    }

    public void UpdageCurSkin()
    {
        Debug.Log("Updating skin");
        Animator anim = skinImage.GetComponent<Animator>();
        anim.runtimeAnimatorController = skins[curSkin].GetPreview() as RuntimeAnimatorController;
    }

    public void ChooseLeftSkin()
    {
        Debug.Log("Going to the left");
        if (curSkin == 0)
        {
            curSkin = skins.Count;
        }
        curSkin--;
        UpdageCurSkin();
    }

    public void ChooseRightSkin()
    {
        Debug.Log("Going to the right");
        curSkin++;
        if (curSkin ==  skins.Count)
        {
            curSkin = 0;
        }
        UpdageCurSkin();
    }
}
