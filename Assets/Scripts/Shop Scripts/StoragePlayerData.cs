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
    private List<WeaponData> weapons;
    [SerializeField]
    private List<SkinData> skins;
    [SerializeField]
    private List<UpgradeData> upgrades;

    public StoragePlayerData(string fileName, List<WeaponData> weapons, List<SkinData> skins, List<UpgradeData> upgrades)
    {
        this.fileName = fileName;
        this.weapons = weapons;
        this.skins = skins;
        this.upgrades = upgrades;
    }

    public string ToJSON()
    {
        return JsonUtility.ToJson(this, true);
    }
}