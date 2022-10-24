using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    private static string startPrefabLocation = "Assets/Prefabs/DefinedStartLocations/StartingProps.prefab";
    private static Vector2 startPrefabPosition = new Vector2(0.0f, -3.0f);
    private static string definedPrefabsLocation = "/Prefabs/DefinedLocations";

    private GameObject startPrefab;
    private GameObject[] definedPrefabs;
    private GameObject playerObject;

    private GameObject lastGeneratePrefab;

    public GameObject generatedObjectsParent;
    public float generationDistance = 10.0f;
    public float xMinShift = 2.0f;
    public float xMaxShift = 4.0f;
    public float yMinDist = -6.0f;
    public float yMaxDist = -2.0f;

    private void Awake()
    {
        startPrefab = AssetDatabase.LoadAssetAtPath(startPrefabLocation, typeof(GameObject)) as GameObject;
        definedPrefabs = GetDefinedPrefabs();        
    }

    private void Start()
    {
        CreateObject(startPrefab, startPrefabPosition);
        playerObject = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        GenerateLevel();
    }

    private GameObject[] GetDefinedPrefabs()
    {
        string[] retrievedObjectPaths = Directory.GetFiles(Application.dataPath + definedPrefabsLocation, "*.prefab", SearchOption.AllDirectories);

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

    private void GenerateLevel()
    {
        PrefabInfo info = lastGeneratePrefab.GetComponent<PrefabInfo>();
        float lastEndPrefab = lastGeneratePrefab.transform.position.x + info.xSize;
        bool shouldGenerate = playerObject.transform.position.x + generationDistance > lastEndPrefab;

        // Debug.Log(playerObject.transform.position.x + generationDistance + " > " + lastEndPrefab + " = " + shouldGenerate);

        if (shouldGenerate)
        {
            float xShift = Random.Range(xMinShift, xMaxShift);
            float yShift = Random.Range(yMinDist, yMaxDist);

            //Debug.Log(xShift + " and " + yShift);

            Vector2 generatedPos = new Vector2(lastEndPrefab + xShift, yShift);
            GameObject objectToGenerate = selectPrefab();

            CreateObject(objectToGenerate, generatedPos);

            //lastGeneratePrefab = Instantiate(objectToGenerate);
            // Debug.Log("!");
        }
    }

    private GameObject selectPrefab()
    {
        int size = definedPrefabs.Length;
        int randomSelected = Random.Range(0, size - 1);

        return definedPrefabs[randomSelected];
    }

    private void CreateObject(GameObject gameObject, Vector2 pos)
    {
        lastGeneratePrefab = Instantiate(gameObject);
        lastGeneratePrefab.transform.position = pos;
        lastGeneratePrefab.transform.parent = generatedObjectsParent.transform;
        
    }
}
