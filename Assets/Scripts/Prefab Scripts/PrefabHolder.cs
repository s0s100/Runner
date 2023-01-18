using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores data about prefab, also responsible for deleting prefabs
public class PrefabHolder : MonoBehaviour
{
    private GameController gameController;

    private const float DESTRUCTION_TIME = 20.0f;

    // X shift before required
    [SerializeField]
    public float XBefore;

    // If it is positive then the previous block should be higher
    [SerializeField]
    public float YBefore;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        if (gameController.IsGameRunning())
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
