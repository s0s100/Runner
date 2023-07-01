using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum NextGeneratedBlockType
{
    Default,
    Coin,
    Boss
}

// Generates playable objects on the screen, contains information about current biome
public class LevelGenerator : MonoBehaviour
{
    // Generation settings and prefabs folders
    private const float generationDistance = 15.0f; // Distance from a camera center from which objects are generated
    private const float Y_CAMERA_SHIFT = 2.0f; //Prefab locaton + this const is the min Y-axis camera location
    private static readonly Vector2 START_PREFAB_POSITION = new Vector2(0.0f, -1.0f);
    private static readonly Vector2 START_PLAYER_POSITION = new Vector2(0.0f, 0.0f);

    // Scene objects
    private GameController gameController;
    private GameObject playerObject;
    private Camera cameraObject;

    // Generation prefabs
    private GameObject lastGeneratedPrefab;
    private GameObject[] startPrefabs;
    private GameObject[] definedPrefabs;
    private GameObject[] definedCoinPrefabs;
    private GameObject[] endPrefabs;
    private GameObject[] startBossPrefabs;
    private GameObject[] bossPrefabs;

    // Generation parent objects
    [SerializeField]
    private GameObject generatedObjectsParent;
    [SerializeField]
    private GameObject generatedEnemyParent;
    [SerializeField]
    private GameObject generatedProjectiles;
    [SerializeField]
    private GameObject generatedAnimations;

    // Attached objects
    [SerializeField]
    private BackgroundController backgroundController;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private BiomeData[] biomeData;

    [Space(50)]
    [Header("Location to generate infinitely")]
    [SerializeField]
    private GameObject onlyBlockToGenerate = null; // Used to test one specified block

    // Biome selection
    Dictionary<NextGeneratedBlockType, GameObject[]> enumToObjLinkage;
    NextGeneratedBlockType nextBlockType;
    private int curActiveBiome; // Biome holder index
    private float coinGenerationChance = 0.25f;
    private bool isBossFight = false;

    private void Awake()
    {
        RandomizeCurBiome();
        GeneratePlayer();
    }

    private void Start()
    {
        GenerateFromLocations(startPrefabs, START_PREFAB_POSITION);
        PopulateLinkageDictionary();

        cameraObject = Camera.main;
        gameController = FindObjectOfType<GameController>();
        nextBlockType = NextGeneratedBlockType.Default;
    }

    private void Update()
    {
        GenerateLocation();
    }

    private void GeneratePlayer()
    {
        playerObject = Instantiate(playerPrefab);
        playerObject.transform.position = START_PLAYER_POSITION;
    }

    private NextGeneratedBlockType DefineNextBlockType()
    {
        if (isBossFight)
        {
            return NextGeneratedBlockType.Boss;
        }

        bool isCoinPrefab = Random.value < coinGenerationChance;
        if (isCoinPrefab)
        {
            return NextGeneratedBlockType.Coin;
        }

        return NextGeneratedBlockType.Default;
    }

    private void GenerateLocation()
    {

        if (ShouldGenerate())
        {
            if (onlyBlockToGenerate == null) 
            {
                nextBlockType = DefineNextBlockType();
                GameObject[] objectToGenerate = enumToObjLinkage[nextBlockType];
                GenerateFromLocations(objectToGenerate);
            } else
            {
                Vector2 generatedPos = GetNextGenerationLocation(onlyBlockToGenerate);
                CreatePrefab(onlyBlockToGenerate, generatedPos);
            }
        }
    }

    private bool ShouldGenerate()
    {
        PrefabData lastPrefabInfo = lastGeneratedPrefab.GetComponent<PrefabData>();
        float lastPrefabX = lastGeneratedPrefab.transform.position.x;
        float lastPrefabY = lastGeneratedPrefab.transform.position.y;

        bool shouldGenerate = cameraObject.transform.position.x + generationDistance
            > lastPrefabX + (lastPrefabInfo.GetXSize() / 2);

        return shouldGenerate;
    }

    private Vector2 GetNextGenerationLocation(GameObject newGenerationObject)
    {
        PrefabData lastPrefabInfo = lastGeneratedPrefab.GetComponent<PrefabData>();
        float lastPrefabX = lastGeneratedPrefab.transform.position.x;
        float lastPrefabY = lastGeneratedPrefab.transform.position.y;

        PrefabData newPrefabInfo = newGenerationObject.GetComponent<PrefabData>();

        float xNewPos = lastPrefabX + newPrefabInfo.XBefore;
        xNewPos += (lastPrefabInfo.GetXSize() / 2);
        xNewPos += lastPrefabInfo.GetXOffset();
        xNewPos -= newPrefabInfo.GetXOffset();

        float yNewPos = lastPrefabY - newPrefabInfo.YBefore;
        yNewPos += lastPrefabInfo.GetYOffset();
        yNewPos -= newPrefabInfo.GetYOffset();
        yNewPos += lastPrefabInfo.YDiff;

        Vector2 generatedPos = new Vector2(xNewPos, yNewPos);
        return generatedPos;
    }

    // Uploads basic and coin prefabs
    private void UploadDefinedPrefabs(BiomeData biome)
    {
        // Upload starting locations
        string path = biome.GetStartLocationsPath();
        startPrefabs = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();

        // Upload default locations
        path = biome.GetLocationsPath();
        definedPrefabs = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();

        // Upload coin locations
        path = biome.GetCoinLocationsPath();
        definedCoinPrefabs = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();

        // Upload ending locations
        path = biome.GetEndLocationsPath();
        endPrefabs = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();

        // Upload start boss locations
        path = biome.GetBossStartLocationsPath();
        startBossPrefabs = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();

        // Upload boss locations
        path = biome.GetBossLocationsPath();
        bossPrefabs = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();
    }

    private void PopulateLinkageDictionary()
    {
        enumToObjLinkage = new Dictionary<NextGeneratedBlockType, GameObject[]>();
        enumToObjLinkage.Add(NextGeneratedBlockType.Default, definedPrefabs);
        enumToObjLinkage.Add(NextGeneratedBlockType.Coin, definedCoinPrefabs);
        // enumToObjLinkage.Add(NextGeneratedBlockType.End, endPrefabs);
        enumToObjLinkage.Add(NextGeneratedBlockType.Boss, bossPrefabs);
        // enumToObjLinkage.Add(NextGeneratedBlockType.None, null);
    }

    private GameObject SelectPrefab(GameObject[] prefabs)
    {
        int size = prefabs.Length;
        int randomSelected = Random.Range(0, size);

        return prefabs[randomSelected];
    }

    private void CreatePrefab(GameObject gameObject, Vector2 pos)
    {
        bool isXShifting = (lastGeneratedPrefab != null);

        lastGeneratedPrefab = Instantiate(gameObject);
        lastGeneratedPrefab.transform.position = pos;

        // Centralize object according to x size
        if (isXShifting)
        {
            PrefabData prefabHolder = lastGeneratedPrefab.GetComponent<PrefabData>();
            float xShift = prefabHolder.GetXSize() / 2;
            lastGeneratedPrefab.transform.position += xShift * Vector3.right;
        }
        
        lastGeneratedPrefab.transform.parent = generatedObjectsParent.transform;
    }

    private void GenerateFromLocations(GameObject[] gameObjects, Vector2 pos)
    {        
        int locationSize = gameObjects.Length;
        int randomIndex = Random.Range(0, locationSize);
        
        CreatePrefab(gameObjects[randomIndex], pos);
    }

    private void GenerateFromLocations(GameObject[] gameObjects)
    {
        int locationSize = gameObjects.Length;
        int randomIndex = Random.Range(0, locationSize);
        GameObject nextLocation = gameObjects[randomIndex];
        Vector2 generatedPos = GetNextGenerationLocation(nextLocation);
        CreatePrefab(nextLocation, generatedPos);
    }

    private void RandomizeCurBiome()
    {
        curActiveBiome = Random.Range(0, biomeData.Length);
        UploadDefinedPrefabs(biomeData[curActiveBiome]);
        backgroundController.SetBiome(biomeData[curActiveBiome]);
        PlayBiomeMusic();
    }

    private void PlayBiomeMusic()
    {
        AudioClip biomeMusic = biomeData[curActiveBiome].GetBiomeMusic();
        AudioController.instance.PlaySpecifiedMusic(biomeMusic);
    }

    public void CreateBoss()
    {
        GameObject boss = biomeData[curActiveBiome].GetBossObject();
        GameObject createdBoss = Instantiate(boss);
        createdBoss.transform.position = playerObject.transform.position + Vector3.right * generationDistance;
        createdBoss.transform.parent = generatedEnemyParent.transform;
    }

    public GameObject GetEnemyParent()
    {
        return generatedEnemyParent;
    }


    public GameObject GetAnimationParent()
    {
        return generatedAnimations;
    }

    // Return min distance for camera
    public float GetMinYPos()
    {
        Collider2D collider = lastGeneratedPrefab.GetComponent<Collider2D>();
        float colliderYSize = collider.bounds.size.y;

        float result = lastGeneratedPrefab.transform.position.y + Y_CAMERA_SHIFT;
        result -= colliderYSize / 2;

        return result;
    }

    public GameObject GetLastGeneratedPrefab()
    {
        return lastGeneratedPrefab;
    }

    public GameObject GetPlayer()
    {
        return playerObject;
    }

    //public void SetGeneratedPrefabParent(GameObject gameObject)
    //{
    //    gameObject.transform.parent = generatedObjectsParent.transform;
    //} 

    public void SetProjectileParent(GameObject newObject)
    {
        newObject.transform.parent = generatedProjectiles.transform;
    }

    public float GetGenerationDistance()
    {
        return generationDistance;
    }

    public void StartBossStage()
    {
        GenerateFromLocations(endPrefabs);
        isBossFight = true;
        this.enabled = false;
        
        // Also disable enemy generation
        gameController.SetEveryEnemyGenerator(false);
    }

    public void ResetPlayerAndLocationPositions()
    {
        playerObject.transform.position = START_PLAYER_POSITION;

        DeleteOldGroundBlocks();
        DeleteEnemies();
        backgroundController.SetBossBackground();
        lastGeneratedPrefab = null;
        GenerateFromLocations(startBossPrefabs, START_PREFAB_POSITION);
        this.enabled = true;
        backgroundController.ReturnDefaultSpeed();

        // Also notify analytics
        AnalyticsController analyticsController = AnalyticsController.instance;
        if (analyticsController != null)
        {
            analyticsController.BossReached();
        }
    }

    private void DeleteOldGroundBlocks()
    {
        foreach (Transform child in generatedObjectsParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void DeleteEnemies()
    {
        foreach (Transform child in generatedEnemyParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    
}