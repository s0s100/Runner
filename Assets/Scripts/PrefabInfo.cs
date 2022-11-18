using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores data about prefab
public class PrefabInfo : MonoBehaviour
{
    [SerializeField]
    public float XSize;

    // Minimal X shift required
    [SerializeField]
    public float XShiftRequired;

    // If it is positive then the previout block should be higher
    [SerializeField]
    public float YBefore;

    // If it is positive then the next block should be higher
    [SerializeField]
    public float YAfter;
}
