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
    private static readonly Vector2 START_PREFAB_POSITION = new Vector2(0.0f, -3.0f);
    private static readonly Vector2 START_PLAYER_POSITION = new Vector2(0.0f, 0.0f);

    // Scene objects
    private GameObject playerObject;
    private Camera cameraObject;

    // Generation prefabs
    private GameObject lastGeneratedPrefab;
    private GameObject[] definedPrefabs;
    private GameObject[] startPrefabs;

    // Generation parent objects
    [SerializeField]
    private GameObject generatedObjectsParent;
    [SerializeField]
    private GameObject generatedEnemyParent;

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

    // Witch generation
    [SerializeField]
    private GameObject witchObject;
    [SerializeField]
    private float witchGenerationTime = 15.0f;
    [SerializeField]
    private float yWitchMinDist = -2.0f;
    [SerializeField]
    private float yWitchMaxDist = 4.0f;
    [SerializeField]
    private float currentWitchGenerationTime = 15.0f;

    // Not used for now ----------
    private float xMinShift = 0.0f;
    private float xMaxShift = 0.0f;
    private float yMinShift = 0.0f;
    private float yMaxShift = 0.0f;
    // ----------------------------

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
        enabled = false;
    }

    private void Update()
    {
        if (IsUpdatingBiome())
        {
            UpdateBiome();
        }

        LevelGeneration();
    }

    private void GeneratePlayer()
    {
        playerObject = Instantiate(playerPrefab);
        playerObject.transform.position = START_PLAYER_POSITION;
    }

    private void LevelGeneration()
    {
        GenerateLevel();

        if (curActiveBiome == 0)
        {
            GenerateWitch();
        }
    }

    private void GenerateLevel()
    {
        PrefabHolder lastPrefabInfo = lastGeneratedPrefab.GetComponent<PrefabHolder>();
        float lastPrefabX = lastGeneratedPrefab.transform.position.x + lastPrefabInfo.XSize;
        bool shouldGenerate = cameraObject.transform.position.x + generationDistance > lastPrefabX;

        if (shouldGenerate)
        {
            GameObject objectToGenerate = SelectPrefab(definedPrefabs);
            PrefabHolder newPrefabInfo = objectToGenerate.GetComponent<PrefabHolder>();

            // Calculate new prefab position prefab position
            float xShift = Random.Range(xMinShift, xMaxShift);
            float yShift = Random.Range(yMinShift, yMaxShift);
            float xNewPos = lastPrefabX + xShift + newPrefabInfo.XBefore + newPrefabInfo.XSize;
            float yNewPos = lastGeneratedPrefab.transform.position.y /*+ lastPrefabInfo.YAfter*/ - newPrefabInfo.YBefore;
            Vector2 generatedPos = new Vector2(xNewPos, yNewPos);
            CreatePrefab(objectToGenerate, generatedPos);
        }
    }

    private void GenerateWitch()
    {
        if (currentWitchGenerationTime > 0)
        {
            currentWitchGenerationTime -= Time.deltaTime;
        } else
        {
            currentWitchGenerationTime = witchGenerationTime;

            bool isReversed = Random.value > 0.5f; // Returns random bool
            bool isSinMoving = Random.value > 0.66f;

            float xDist;
            if (isReversed)
            {
                xDist = -generationDistance + cameraObject.transform.position.x;
            } else
            {
                xDist = generationDistance + cameraObject.transform.position.x;
            }

            float yDist = Random.Range(yWitchMinDist, yWitchMaxDist);
            yDist += cameraObject.transform.position.y;

            Vector2 witchPos = new Vector2(xDist, yDist);
            GameObject newWitch = Instantiate(witchObject);

            // Reverse witch if possible
            if (isReversed)
            {
                Witch witchControl = newWitch.GetComponent<Witch>();
                witchControl.SetLeftToRightMovement();
                witchControl.SetSinMovement();
                newWitch.transform.Rotate(0.0f, 180.0f, 0.0f); // Rotate to make her move in the other direction
            }

            newWitch.transform.position = witchPos;
            newWitch.transform.parent = generatedEnemyParent.transform;
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
        NotifyBackground();
    }

    private void UploadDefinedPrefabs(BiomeHolder biome)
    {
        string path = biome.PrefabsPath;
        definedPrefabs = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();
    }

    private GameObject SelectPrefab(GameObject[] prefabs)
    {
        int size = prefabs.Length;
        int randomSelected = Random.Range(0, size);

        return prefabs[randomSelected];
    }

    private void CreatePrefab(GameObject gameObject, Vector2 pos)
    {
        lastGeneratedPrefab = Instantiate(gameObject);
        lastGeneratedPrefab.transform.position = pos;
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
        NotifyBackground();
    }

    private void NotifyBackground()
    {
        backgroundController.SetBiome(biomeHolders[curActiveBiome]);
    }

    public GameObject GetEnemyParent()
    {
        return generatedEnemyParent;
    }

    // Return min distance for camera
    public float GetMinYPos()
    {
        return lastGeneratedPrefab.transform.position.y + Y_CAMERA_SHIFT;
    }

    public GameObject GetLastGeneratedPrefab()
    {
        return lastGeneratedPrefab;
    }

    public GameObject GetPlayer()
    {
        return playerObject;
    }
}
