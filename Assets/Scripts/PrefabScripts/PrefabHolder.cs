using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores data about prefab, also responsible for deleting prefabs
public class PrefabHolder : MonoBehaviour
{
    private const float DESTRUCTION_TIME = 20.0f;

    [SerializeField]
    public float XSize;

    // X shift before required
    [SerializeField]
    public float XShiftRequired;

    // If it is positive then the previout block should be higher
    [SerializeField]
    public float YBefore;

    // If it is positive then the next block should be higher
    [SerializeField]
    public float YAfter;

    private void Start()
    {
        Destroy(gameObject, DESTRUCTION_TIME);
    }
}
