using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/StoragePlayerData", order = 1)]
public class StoragePlayerData : ScriptableObject
{
    [SerializeField]
    private string fileName;
    [SerializeField]
    private List<SkinData> skins;
    [SerializeField]
    private List<UpgradeData> upgrades;

    public StoragePlayerData(string fileName, List<SkinData> skins, List<UpgradeData> upgrades)
    {
        this.fileName = fileName;
        this.skins = skins;
        this.upgrades = upgrades;
    }

    public string ToJSON()
    {
        return JsonUtility.ToJson(this, true);
    }

    public List<SkinData> GetSkinList()
    {
        return skins;
    }

    public SkinData GetIndexSkin(int index)
    {
        if (index < skins.Count)
        {
            return skins[index];
        }

        return null;
    }
}