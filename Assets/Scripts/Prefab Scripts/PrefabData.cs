using UnityEngine;

// Stores data about prefab, also responsible for deleting prefabs
public class PrefabData : MonoBehaviour
{
    private GameController gameController;

    [SerializeField]
    [Header("Default value is 30 seconds")]
    private float destructionTime = 30.0f;

    // X shift before required
    [SerializeField]
    public float XBefore;

    // If it is positive then the previous block should be higher
    [SerializeField]
    public float YBefore;

    // Y difference between start and end of the block;
    [SerializeField]
    public float YDiff;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        if (gameController.IsGameRunning())
        {
            Destroy(gameObject, destructionTime);
        }
    }

    public float GetXSize()
    {
        return GetComponent<BoxCollider2D>().bounds.size.x;
    }

    public float GetXOffset()
    {
        return GetComponent<BoxCollider2D>().offset.x;
    }

    public float GetYOffset()
    {
        return GetComponent<BoxCollider2D>().offset.y;
    }

    public void LateDestroy()
    {
        Destroy(gameObject, destructionTime);
    }
}
