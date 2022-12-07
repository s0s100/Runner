using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control background images for different biomes
public class BackgroundController : MonoBehaviour
{
    private float backgroundFrontYShift = -1.5f;

    [SerializeField]
    private GameObject backgroundFrontGreen;
    [SerializeField]
    private GameObject backgroundBackGreen;
    [SerializeField]
    private GameObject backgroundFrontRed;
    [SerializeField]
    private GameObject backgroundBackRed;
    [SerializeField]
    private GameObject backgroundParent;

    private List<GameObject> backs = new List<GameObject>(); // Back background objects
    private List<GameObject> fronts = new List<GameObject>(); // Front background objects
    private GameController controller;
    private float frontMoveSpeed; // Switches regularly
    private float backMoveSpeed; // Switches if biome is changed


    private Biome curBiome;

    private void Start()
    {
        controller = FindObjectOfType<GameController>();
        frontMoveSpeed = controller.GetGameSpeed();
        backMoveSpeed = controller.GetGameSpeed();
    }

    private void Update()
    {
        MoveFront();
    }

    private void MoveFront()
    {

    }

    // Set current biome and set images accordingly
    public void SetBiome(Biome biome)
    {
        curBiome = biome;
        generateBack();
    }

    // Set current biome and update background accordingly
    public void UpdateBiome(Biome newBiome)
    {

    }

    // Generates backaccording to current biome
    private void generateBack()
    {
        GameObject newBack = null;
        switch (curBiome)
        {
            case Biome.Green:
                {
                    newBack = Instantiate(backgroundBackGreen);
                    break;
                }
            case Biome.Red:
                {
                    newBack = Instantiate(backgroundBackRed);
                    break;
                }
        }
        
        if (backs.Count == 0)
        {
            newBack.transform.position = Vector3.zero;
        }
        else
        {
            float backSize = defineObjBoundaries(newBack);
            Vector2 backPos = new Vector2(backSize, 0);
            newBack.transform.position = backPos;
        }

        newBack.transform.parent = backgroundParent.transform;
        backs.Add(newBack);
    }

    // Uses render boundaries
    private float defineObjBoundaries(GameObject obj)
    {
        float result = obj.GetComponent<Renderer>().bounds.size.x;
        return result;
    }
}
