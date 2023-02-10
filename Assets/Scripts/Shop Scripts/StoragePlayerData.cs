using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/StoragePlayerData", order = 1)]
public class StoragePlayerData : ScriptableObject
{
    [SerializeField]
    private List<WeaponData> weapons;
    [SerializeField]
    private List<SkinData> skins;
    [SerializeField]
    private List<UpgradeData> upgrades;

    public string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }
}