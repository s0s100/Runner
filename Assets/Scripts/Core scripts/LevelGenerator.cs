using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    private const string START_PREFAB_LOCATION = "Assets/Prefabs/Locations/DefinedStartLocations/StartingProps.prefab";
    private const string DEFINED_PREFABS_LOCATION = "/Prefabs/Locations/DefinedGreenLocations";
    private static readonly Vector2 START_PREFAB_POSITION = new Vector2(0.0f, -3.0f);

    private GameObject startPrefab;
    private GameObject[] definedPrefabs;
    private GameObject playerObject;
    private GameObject cameraObject;

    private GameObject lastGeneratePrefab;

    // Prefab generation
    [SerializeField]
    private GameObject generatedObjectsParent;
    [SerializeField]
    private GameObject generatedEnemyParent;

    private float generationDistance = 10.0f; // Distance from a camera center from which objects are generated
    private float xMinShift = 0.0f;
    private float xMaxShift = 0.0f;
    private float yMinShift = 0.0f;
    private float yMaxShift = 0.0f;

    // Witch generation (uses same generation distance)
    [SerializeField]
    private GameObject witchObject;

    private float witchGenerationTime = 10.0f;
    private float yWitchMinDist = -2.0f;
    private float yWitchMaxDist = 4.0f;
    private float currentWitchGenerationTime = 0.0f; // = 10.0f;

    private void Awake()
    {
        startPrefab = AssetDatabase.LoadAssetAtPath(START_PREFAB_LOCATION, typeof(GameObject)) as GameObject;
        definedPrefabs = GetFolderPrefabs(DEFINED_PREFABS_LOCATION);
    }

    private void Start()
    {
        CreatePrefab(startPrefab, START_PREFAB_POSITION); 
        playerObject = GameObject.FindWithTag("Player");
        cameraObject = GameObject.FindWithTag("MainCamera");
        enabled = false;
    }

    private void Update()
    {
        GenerateLevel();
        GenerateWitch();
    }

    private void GenerateLevel()
    {
        PrefabHolder lastPrefabInfo = lastGeneratePrefab.GetComponent<PrefabHolder>();
        float lastPrefabX = lastGeneratePrefab.transform.position.x + lastPrefabInfo.XSize;
        bool shouldGenerate = cameraObject.transform.position.x + generationDistance > lastPrefabX;

        if (shouldGenerate)
        {
            GameObject objectToGenerate = SelectPrefab();
            PrefabHolder newPrefabInfo = objectToGenerate.GetComponent<PrefabHolder>();

            // Calculate new prefab position prefab position
            float xShift = Random.Range(xMinShift, xMaxShift);
            float yShift = Random.Range(yMinShift, yMaxShift);
            float xNewPos = lastPrefabX + xShift + newPrefabInfo.XShiftRequired + newPrefabInfo.XSize;
            float yNewPos = lastGeneratePrefab.transform.position.y + lastPrefabInfo.YAfter - newPrefabInfo.YBefore;

            // Debug.Log("Generated: " + xNewPos + " " + yNewPos);

            Vector2 generatedPos = new Vector2(xNewPos , yNewPos);
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

    private GameObject SelectPrefab()
    {
        int size = definedPrefabs.Length;
        int randomSelected = Random.Range(0, size - 1);

        return definedPrefabs[randomSelected];
    }

    private void CreatePrefab(GameObject gameObject, Vector2 pos)
    {
        lastGeneratePrefab = Instantiate(gameObject);
        lastGeneratePrefab.transform.position = pos;
        lastGeneratePrefab.transform.parent = generatedObjectsParent.transform;
    }
}
