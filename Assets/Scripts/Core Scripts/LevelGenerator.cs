using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

// Generates playable objects on the screen, contains information about current biome
public class LevelGenerator : MonoBehaviour
{
    // Generation settings and prefabs folders
    private const string BIOME_FOLDER_LOCATION = "Prefabs/Biomes";
    private const string START_PREFABS_LOCATION = "Prefabs/Locations/DefinedStartLocations";
    private const float generationDistance = 10.0f; // Distance from a camera center from which objects are generated
    private const float Y_CAMERA_SHIFT = 2.0f; //Prefab locaton + this const is the min Y-axis camera location
    private static readonly Vector2 START_PREFAB_POSITION = new Vector2(0.0f, -1.0f);
    private static readonly Vector2 START_PLAYER_POSITION = new Vector2(0.0f, 0.0f);

    // Scene objects
    private GameObject playerObject;
    private Camera cameraObject;

    // Generation prefabs
    private GameObject lastGeneratedPrefab;
    private GameObject[] definedPrefabs;
    private GameObject[] definedCoinPrefabs;
    private GameObject[] startPrefabs;

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
    private BiomeHolder[] biomeHolders;
    
    // Biome selection
    [SerializeField]
    private float timeBeforeNewBiome = 90.0f;
    private float curBiomeChangeTimer = 0.0f;
    private int curActiveBiome; // Biome holder index

    // Coin generation
    private float coinGenerationChance = 0.25f; 

    private void Awake()
    {
        RandomizeCurBiome();
        startPrefabs = Resources.LoadAll(START_PREFABS_LOCATION, typeof(GameObject)).Cast<GameObject>().ToArray();
    }

    private void Start()
    {
        GenerateStartLocation();
        GeneratePlayer();
        cameraObject = Camera.main;
        // enabled = false;
    }

    private void Update()
    {
        if (IsUpdatingBiome())
        {
            UpdateBiome();
        }

        GenerateLocation();
    }

    private void GeneratePlayer()
    {
        playerObject = Instantiate(playerPrefab);
        playerObject.transform.position = START_PLAYER_POSITION;
    }

    private void GenerateLocation()
    {
        PrefabHolder lastPrefabInfo = lastGeneratedPrefab.GetComponent<PrefabHolder>();
        float lastPrefabX = lastGeneratedPrefab.transform.position.x;
        float lastPrefabY = lastGeneratedPrefab.transform.position.y;

        bool shouldGenerate = cameraObject.transform.position.x + generationDistance 
            > lastPrefabX + (lastPrefabInfo.GetXSize() / 2);

        if (shouldGenerate)
        {
            // Decide which type of locations will be generated
            bool isCoinPrefab = Random.value < coinGenerationChance;
            GameObject objectToGenerate;
            if (isCoinPrefab)
            {
                objectToGenerate = SelectPrefab(definedCoinPrefabs);
            }  else
            {
                objectToGenerate = SelectPrefab(definedPrefabs);
            }
            
            PrefabHolder newPrefabInfo = objectToGenerate.GetComponent<PrefabHolder>();

            // Calculate new prefab position prefab position
            float xNewPos = lastPrefabX + newPrefabInfo.XBefore;
            xNewPos += (lastPrefabInfo.GetXSize() / 2);
            xNewPos += lastPrefabInfo.GetXOffset();
            xNewPos -= newPrefabInfo.GetXOffset();

            float yNewPos = lastPrefabY - newPrefabInfo.YBefore;
            yNewPos += lastPrefabInfo.GetYOffset();
            yNewPos -= newPrefabInfo.GetYOffset();
            yNewPos += lastPrefabInfo.YDiff;

            Vector2 generatedPos = new Vector2(xNewPos, yNewPos);

            CreatePrefab(objectToGenerate, generatedPos);
        }
    }
    
    private bool IsUpdatingBiome()
    {
        curBiomeChangeTimer += Time.deltaTime;
        if (curBiomeChangeTimer >= timeBeforeNewBiome)
        {
            curBiomeChangeTimer = 0.0f;
            return true;
        }
        return false;
    }
    
    private void UpdateBiome()
    {
        if (curActiveBiome == 0)
        {
            curActiveBiome = 1;
        } else
        {
            curActiveBiome = 0;
        }

        UploadDefinedPrefabs(biomeHolders[curActiveBiome]);
        backgroundController.UpdateBiome(biomeHolders[curActiveBiome]);
    }

    // Uploads basic and coin prefabs
    private void UploadDefinedPrefabs(BiomeHolder biome)
    {
        string path = biome.PrefabsPath;
        definedPrefabs = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();

        // Also upload coin prefabs
        path = biome.CoinPrefabPath;
        definedCoinPrefabs = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();
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
            PrefabHolder prefabHolder = lastGeneratedPrefab.GetComponent<PrefabHolder>();
            float xShift = prefabHolder.GetXSize() / 2;
            lastGeneratedPrefab.transform.position += xShift * Vector3.right;
        }
        
        lastGeneratedPrefab.transform.parent = generatedObjectsParent.transform;
    }

    private void GenerateStartLocation()
    {
        string path = biomeHolders[curActiveBiome].StartPrefabsPath;
        GameObject[] startLocations = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();

        int locationSize = startLocations.Length;
        int randomIndex = Random.Range(0, locationSize);

        CreatePrefab(startLocations[randomIndex], START_PREFAB_POSITION);
    }

    private void RandomizeCurBiome()
    {
        curActiveBiome = Random.Range(0, biomeHolders.Length);
        UploadDefinedPrefabs(biomeHolders[curActiveBiome]);
        backgroundController.SetBiome(biomeHolders[curActiveBiome]);
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

    public void SetProjectileParent(GameObject newObject)
    {
        newObject.transform.parent = generatedProjectiles.transform;
    }

    public float GetGenerationDistance()
    {
        return generationDistance;
    }
}
