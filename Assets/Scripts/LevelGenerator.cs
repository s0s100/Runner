using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    private static string startPrefabLocation = "Assets/Prefabs/DefinedStartLocations/StartingProps.prefab";
    private static string definedPrefabsLocation = "/Prefabs/DefinedLocations";

    private GameObject startPrefab;
    private GameObject[] definedPrefabs;

    private Vector2 lastEndPoint;

    public float xDistShift = 2.5f;
    public float yDistShift = 1.0f;
    public float yMinDist = 0.0f;
    public float yMaxDist = 2.0f;

    private void Awake()
    {
        startPrefab = AssetDatabase.LoadAssetAtPath(startPrefabLocation, typeof(GameObject)) as GameObject;
        // definedPrefabs = AssetDatabase.LoadAllAssetsAtPath(definedPrefabsLocation) as GameObject[];
        definedPrefabs = GetDefinedPrefabs();

        //if (definedPrefabs != null)
        //{
        //    Debug.Log(definedPrefabs.Length);
        //} else
        //{
        //    Debug.Log("?");
        //}
        
    }

    private void Start()
    {
        GameObject generatedObject = Instantiate(startPrefab);

        // Test
        //GameObject testObj = Instantiate(definedPrefabs[0]);
    }

    //private GameObject[] GetDefinedPrefabs()
    //{
    //    Object[] retrievedObjects = AssetDatabase.LoadAllAssetsAtPath(definedPrefabsLocation);

    //    if (retrievedObjects == null)
    //    {
    //        return null;
    //    }

    //    int size = retrievedObjects.Length;
    //    GameObject[] result = new GameObject[size];

    //    Object o;
    //    for (int i = 0; i < size; i++)
    //    {
    //        o = retrievedObjects[i];
    //        result[i] = o as GameObject;
    //    }

    //    return result;
    //}

    private GameObject[] GetDefinedPrefabs()
    {
        string[] retrievedObjectPaths = Directory.GetFiles(Application.dataPath + definedPrefabsLocation, "*.prefab", SearchOption.AllDirectories);

        Debug.Log(retrievedObjectPaths.Length);
        Debug.Log(Application.dataPath);

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

            //Debug.Log(assetPath);
            //if (result[i] == null)
            //{
            //    Debug.Log("Not correct path");
            //}
        }

        return result;
    }

}
