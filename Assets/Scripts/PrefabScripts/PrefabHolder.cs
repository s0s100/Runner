using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores data about prefab, also responsible for deleting prefabs
public class PrefabHolder : MonoBehaviour
{
    private const float DESTRUCTION_TIME = 20.0f;

    [SerializeField]
    public bool IsItStartPrefab = false;

    // X shift before required
    [SerializeField]
    public float XBefore;

    // If it is positive then the previous block should be higher
    [SerializeField]
    public float YBefore;

    private void Start()
    {

        if (!IsItStartPrefab)
        {
            Destroy(gameObject, DESTRUCTION_TIME);
        }
    }

    public float GetXSize()
    {
        return GetComponent<BoxCollider2D>().bounds.size.x;
    }

    public void LateDestroy()
    {
        Destroy(gameObject, DESTRUCTION_TIME);
    }
}
