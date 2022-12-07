using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control background images for different biomes
public class BackgroundController : MonoBehaviour
{
    private const float FRONT_SPEED_MODIFIER = 8.0f;
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
        frontMoveSpeed = controller.GetGameSpeed() / 2;
        backMoveSpeed = controller.GetGameSpeed() * 4;
        this.enabled = false;
    }

    private void Update()
    {
        MoveFront();
        MoveBack();
    }

    private void MoveFront()
    {
        if (fronts.Count > 1)
        {
            float increment = 1.0f;
            if (fronts.Count > 2)
            {
                increment = FRONT_SPEED_MODIFIER;
            }
            fronts[0].transform.position -= Vector3.right * frontMoveSpeed * Time.deltaTime * increment;
            fronts[1].transform.position -= Vector3.right * frontMoveSpeed * Time.deltaTime * increment;

            float frontDist = defineObjBoundaries(fronts[0]);
            if (fronts[0].transform.localPosition.x <= -frontDist)
            {
                Destroy(fronts[0]);
                fronts.RemoveAt(0);
            }

        } else
        {
            generateFront();
        }
    }

    private void MoveBack()
    {
        if (backs.Count > 1)
        {
            backs[0].transform.position -= Vector3.right * backMoveSpeed * Time.deltaTime;
            backs[1].transform.position -= Vector3.right * backMoveSpeed * Time.deltaTime;

            float backDist = defineObjBoundaries(backs[0]);
            if (backs[0].transform.localPosition.x <= -backDist)
            {
                Destroy(backs[0]);
                backs.RemoveAt(0);
            }
        }
    }

    // Set current biome and set images accordingly
    public void SetBiome(Biome biome)
    {
        Debug.Log("I was called");
        curBiome = biome;
        Debug.Log(biome);
        generateBack();
        generateFront();
    }

    public void generateFront()
    {
        GameObject newFront = null;
        switch (curBiome)
        {
            case Biome.Green:
                {
                    newFront = Instantiate(backgroundFrontGreen);
                    break;
                }
            case Biome.Red:
                {
                    newFront = Instantiate(backgroundFrontRed);
                    break;
                }
        }

        newFront.transform.parent = backgroundParent.transform;
        if (fronts.Count == 0)
        {
            newFront.transform.localPosition = new Vector2(0, backgroundFrontYShift);
        }
        else
        {
            GameObject lastObject = fronts[fronts.Count - 1];
            float frontSize = defineObjBoundaries(lastObject);
            Vector2 backPos = new Vector2(frontSize, backgroundFrontYShift);
            newFront.transform.localPosition = backPos;
        }
        
        fronts.Add(newFront);
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

        newBack.transform.parent = backgroundParent.transform;
        if (backs.Count == 0)
        {
            newBack.transform.localPosition = Vector3.zero;
        }
        else
        {
            GameObject lastObject = backs[backs.Count - 1];
            float backSize = defineObjBoundaries(lastObject);
            Vector2 backPos = new Vector2(backSize, 0);
            newBack.transform.localPosition = backPos;
        }
        
        backs.Add(newBack);
    }

    // Uses render boundaries
    private float defineObjBoundaries(GameObject obj)
    {
        float result = obj.GetComponent<Collider2D>().bounds.size.x;
        return result;
    }
}
