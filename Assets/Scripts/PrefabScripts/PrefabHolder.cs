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

    public float XSize { get; private set; }

    private void Start()
    {
        XSize = GetComponent<Collider2D>().bounds.size.x;

        if (!IsItStartPrefab)
        {
            Destroy(gameObject, DESTRUCTION_TIME);
        }
    }

    public void LateDestroy()
    {
        Destroy(gameObject, DESTRUCTION_TIME);
    }
}
