using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public enum Biome
{
    Green, Red
}

public class BiomeInstanse
{
    
}

// Generates playable objects on the screen, contains information about current biome
public class LevelGenerator : MonoBehaviour
{
    // Generation settings and prefabs folders
    private const string DEFINED_GREEN_PREFABS_LOCATION = "/Prefabs/Locations/DefinedGreenLocations";
    private const string DEFINED_RED_PREFABS_LOCATION = "/Prefabs/Locations/DefinedRedLocations";
    private const string START_PREFABS_LOCATION = "/Prefabs/Locations/DefinedStartLocations";
    private const float generationDistance = 10.0f; // Distance from a camera center from which objects are generated
    private const float Y_CAMERA_SHIFT = 2.0f; //Prefab locaton + this const is the min Y-axis camera location
    private static readonly Vector2 START_PREFAB_POSITION = new Vector2(0.0f, -3.0f);
    private static readonly Vector2 START_PLAYER_POSITION = new Vector2(0.0f, 0.0f);

    // Scene objects
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject playerObject;
    private Camera cameraObject;
    
    // Generation prefabs
    private GameObject lastGeneratePrefab;
    private GameObject[] definedGreenPrefabs;
    private GameObject[] definedRedPrefabs;
    private GameObject[] startPrefabs;

    // Generation parent objects
    [SerializeField]
    private GameObject generatedObjectsParent;
    [SerializeField]
    private GameObject generatedEnemyParent;

    // Biome settings
    [SerializeField]
    private float timeBeforeNewBiome = 90.0f;
    [SerializeField]
    private BackgroundController backgroundController;
    private Biome curBiome = Biome.Red;
    private float curBiomeChangeTimer = 0.0f;
    

    // Not used for now ----------
    private float xMinShift = 0.0f;
    private float xMaxShift = 0.0f;
    private float yMinShift = 0.0f;
    private float yMaxShift = 0.0f;
    // ----------------------------

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

    private void Awake()
    {
        definedGreenPrefabs = GetFolderPrefabs(DEFINED_GREEN_PREFABS_LOCATION);
        definedRedPrefabs = GetFolderPrefabs(DEFINED_RED_PREFABS_LOCATION);

        startPrefabs = GetFolderPrefabs(START_PREFABS_LOCATION);
    }

    private void Start()
    {
        CreatePrefab(SelectAndNotifyBiome(), START_PREFAB_POSITION);

        playerObject = Instantiate(playerPrefab);
        playerObject.transform.position = START_PLAYER_POSITION;
        cameraObject = Camera.main;

        enabled = false;
    }

    private void Update()
    {
        if (IsUpdatingBiome())
        {
            UpdateBiome();
        }

        switch (curBiome)
        {
            case Biome.Green:
                {
                    GenerateLevel(definedGreenPrefabs);
                    GenerateWitch();
                    break;
                }
            case Biome.Red:
                {
                    GenerateLevel(definedRedPrefabs);
                    break;
                }
        }
    }

    private void GenerateLevel(GameObject[] definedPrefabs)
    {
            PrefabHolder lastPrefabInfo = lastGeneratePrefab.GetComponent<PrefabHolder>();
            float lastPrefabX = lastGeneratePrefab.transform.position.x + lastPrefabInfo.XSize;
            bool shouldGenerate = cameraObject.transform.position.x + generationDistance > lastPrefabX;

            if (shouldGenerate)
            {
                GameObject objectToGenerate = SelectPrefab(definedPrefabs);
                PrefabHolder newPrefabInfo = objectToGenerate.GetComponent<PrefabHolder>();

                // Calculate new prefab position prefab position
                float xShift = Random.Range(xMinShift, xMaxShift);
                float yShift = Random.Range(yMinShift, yMaxShift);
                float xNewPos = lastPrefabX + xShift + newPrefabInfo.XShiftRequired + newPrefabInfo.XSize;
                float yNewPos = lastGeneratePrefab.transform.position.y + lastPrefabInfo.YAfter - newPrefabInfo.YBefore;

                // Debug.Log("Generated: " + xNewPos + " " + yNewPos);

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

    // Checks whether biome requires to be changed
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

    // Updates block use and background
    private void UpdateBiome()
    {
        if (curBiome == Biome.Green)
        {
            curBiome = Biome.Red;
        } else
        {
            curBiome = Biome.Green;
        }
        backgroundController.SetBiome(curBiome);
    }

    // Uploads objects from the folder
    private GameObject[] GetFolderPrefabs(string path)
    {
        string[] retrievedObjectPaths = Directory.GetFiles(Application.dataPath + path, "*.prefab", SearchOption.AllDirectories);

        if (retrievedObjectPaths == null)
        {
            return null;
        }

        int size = retrievedObjectPaths.Length;
        GameObject[] result = new GameObject[size];

        string prefabPath;
        string assetPath;
        for (int i = 0; i < size; i++)
        {
            prefabPath = retrievedObjectPaths[i];
            assetPath = "Assets" + prefabPath.Replace(Application.dataPath, "");
            assetPath = assetPath.Replace('\\', '/');

            result[i] = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
        }

        return result;
    }

    private GameObject SelectPrefab(GameObject[] prefabs)
    {
        int size = prefabs.Length;
        int randomSelected = Random.Range(0, size);

        return prefabs[randomSelected];
    }

    private void CreatePrefab(GameObject gameObject, Vector2 pos)
    {
        lastGeneratePrefab = Instantiate(gameObject);
        lastGeneratePrefab.transform.position = pos;
        lastGeneratePrefab.transform.parent = generatedObjectsParent.transform;
    }

    // Selects and defines biomes, also notifies background controller
    private GameObject SelectAndNotifyBiome()
    {
        GameObject startPrefab = SelectPrefab(startPrefabs);
        PrefabHolder prefabHolder = startPrefab.GetComponent<PrefabHolder>();
        curBiome = prefabHolder.curBiome;
        backgroundController.SetBiome(curBiome);

        return startPrefab;
    }

    public GameObject GetEnemyParent()
    {
        return generatedEnemyParent;
    }

    // Return min distance for camera
    public float GetMinYPos()
    {
        return lastGeneratePrefab.transform.position.y + Y_CAMERA_SHIFT;
    }

    public GameObject GetLastGeneratedPrefab()
    {
        return lastGeneratePrefab;
    }

    public GameObject getPlayer()
    {
        return playerObject;
    }
}
