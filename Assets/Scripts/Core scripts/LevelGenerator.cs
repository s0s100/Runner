using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    private const string START_PREFAB_LOCATION = "Assets/Prefabs/DefinedStartLocations/StartingProps.prefab";
    private const string DEFINED_PREFABS_LOCATION = "/Prefabs/DefinedLocations";
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
    private float xMinShift = 0.0f; // 2.0f;
    private float xMaxShift = 0.0f; // 4.0f;
    private float yMinShift = 0.0f; // -6.0f;
    private float yMaxShift = 0.0f; // -2.0f; 

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
        definedPrefabs = GetDefinedPrefabs();
    }

    private void Start()
    {
        Createprefab(startPrefab, START_PREFAB_POSITION); 
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
        PrefabInfo lastPrefabInfo = lastGeneratePrefab.GetComponent<PrefabInfo>();
        float lastPrefabX = lastGeneratePrefab.transform.position.x + lastPrefabInfo.XSize;
        bool shouldGenerate = cameraObject.transform.position.x + generationDistance > lastPrefabX;

        if (shouldGenerate)
        {
            GameObject objectToGenerate = selectPrefab();
            PrefabInfo newPrefabInfo = objectToGenerate.GetComponent<PrefabInfo>();

            // Calculate new prefab position prefab position
            float xShift = Random.Range(xMinShift, xMaxShift);
            float yShift = Random.Range(yMinShift, yMaxShift);
            float xNewPos = lastPrefabX + xShift + newPrefabInfo.XShiftRequired + newPrefabInfo.XSize;
            float yNewPos = lastGeneratePrefab.transform.position.y + lastPrefabInfo.YAfter + newPrefabInfo.YBefore;

            Debug.Log("Generated: " + xNewPos + " " + yNewPos);

            Vector2 generatedPos = new Vector2(xNewPos , yNewPos);
            Createprefab(objectToGenerate, generatedPos);
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
                WitchControl witchControl = newWitch.GetComponent<WitchControl>();
                witchControl.SetLeftToRightMovement(true);
                newWitch.transform.Rotate(0.0f, 180.0f, 0.0f); // Rotate to make her move in the other direction
            }

            newWitch.transform.position = witchPos;
            newWitch.transform.parent = generatedEnemyParent.transform;
        }
    }

    private GameObject[] GetDefinedPrefabs()
    {
        string[] retrievedObjectPaths = Directory.GetFiles(Application.dataPath + DEFINED_PREFABS_LOCATION, "*.prefab", SearchOption.AllDirectories);

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

    private GameObject selectPrefab()
    {
        int size = definedPrefabs.Length;
        int randomSelected = Random.Range(0, size - 1);

        return definedPrefabs[randomSelected];
    }

    private void Createprefab(GameObject gameObject, Vector2 pos)
    {
        lastGeneratePrefab = Instantiate(gameObject);
        lastGeneratePrefab.transform.position = pos;
        lastGeneratePrefab.transform.parent = generatedObjectsParent.transform;
    }
}
