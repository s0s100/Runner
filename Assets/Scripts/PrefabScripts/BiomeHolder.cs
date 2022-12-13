using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome
{
    Green, Red
}

public class BiomeHolder : MonoBehaviour
{
    [SerializeField]
    public Biome biome;

    [SerializeField]
    public string PrefabPath;

    [SerializeField]
    public GameObject BackgroundBack;

    [SerializeField]
    public GameObject BackgroundFront;
}
