using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BiomeData", menuName = "ScriptableObjects/BiomeData", order = 2)]
public class BiomeData : ScriptableObject
{
    private static string PATH_TO_START_LOCATIONS = "StartLocations";
    private static string PATH_TO_LOCATIONS = "Locations";
    private static string PATH_TO_COIN_LOCATIONS = "CoinLocations";

    [SerializeField]
    private string locationsGeneralPath;

    [SerializeField]
    private GameObject backgroundBack;

    [SerializeField]
    private GameObject backgroundFront;

    public string GetStartLocationsPath()
    {
        string path = Path.Combine(locationsGeneralPath, PATH_TO_START_LOCATIONS);
        return path;
    }

    public string GetLocationsPath()
    {
        string path = Path.Combine(locationsGeneralPath, PATH_TO_LOCATIONS);
        return path;
    }

    public string GetCoinLocationsPath()
    {
        string path = Path.Combine(locationsGeneralPath, PATH_TO_COIN_LOCATIONS);
        return path;
    }

    public GameObject GetBackgroundBack()
    {
        return backgroundBack;
    }

    public GameObject GetBackgroundFront()
    {
        return backgroundFront;
    }
}