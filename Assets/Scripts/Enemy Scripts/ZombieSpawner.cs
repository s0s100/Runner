using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private const float MIN_SPAWN_DIST = 2.5f;
    private const float MAX_SPAWN_DIST = 8.0f;

    private bool isSpawned = false;
    private float requiredDist;

    private GameObject enemyFolder;
    [SerializeField]
    private GameObject zombiePrefab;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        requiredDist = Random.Range(MIN_SPAWN_DIST, MAX_SPAWN_DIST);
        GetEnemyFolder();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSpawning() && !isSpawned)
        {
            GenerateZombie();
            enabled = false;
        }
    }

    // Checks the distance between player and the grave, if reached specific distance return true
    private bool IsSpawning()
    {
        float playerXPos = player.transform.position.x;
        float zombieXPos = transform.position.x;

        if (zombieXPos - playerXPos <= requiredDist)
        {
            return true;
        }

        return false;
    }

    private void GetEnemyFolder()
    {
        LevelGenerator generator = FindObjectOfType<LevelGenerator>();
        enemyFolder = generator.GetEnemyParent();
    }

    private void GenerateZombie()
    {
        GameObject newObject = Instantiate(zombiePrefab);
        newObject.transform.position = transform.position;
        newObject.transform.parent = enemyFolder.transform;
        isSpawned = false;
    }
}
