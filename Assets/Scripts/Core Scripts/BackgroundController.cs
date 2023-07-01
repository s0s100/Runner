using System.Collections.Generic;
using UnityEngine;

// Control background images and effects for different biomes
public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    private GameObject backgroundParent;

    // Background images
    private GameObject backImage;
    private List<GameObject> frontsImages = new List<GameObject>();
    private BiomeData curBiomeHolder;
    private GameController controller;
    private float frontYShift = 0; // Middle background Y shift
    private float frontMoveSpeed;

    private bool isBoss = false;

    private void Start()
    {
        controller = FindObjectOfType<GameController>();
        frontMoveSpeed = controller.GetGameSpeed() / 2;
        this.enabled = false;
    }

    private void Update()
    {
        MoveFront();
    }

    private void MoveFront()
    {
        if (frontsImages.Count > 1)
        {
            frontsImages[0].transform.position -= Vector3.right * frontMoveSpeed * Time.deltaTime;
            frontsImages[1].transform.position -= Vector3.right * frontMoveSpeed * Time.deltaTime;

            float frontDist = DefineObjBoundaries(frontsImages[0]);
            if (frontsImages[0].transform.localPosition.x <= -frontDist)
            {
                Destroy(frontsImages[0]);
                frontsImages.RemoveAt(0);
            }

        } else
        {
            if (!isBoss)
            {
                AddFrontImage(curBiomeHolder.GetBackgroundFront());
            } else
            {
                AddFrontImage(curBiomeHolder.GetBossBackgroundFront());
            }
        }
    }
    
    public void SetBiome(BiomeData newBiome)
    {
        curBiomeHolder = newBiome;
        SetBackImage(curBiomeHolder.GetBackgroundBack());
        SetFrontImage(curBiomeHolder.GetBackgroundFront());
    }

    public void UpdateBiome(BiomeData newBiome)
    {
        curBiomeHolder = newBiome;
    }

    private void SetFrontImage(GameObject frontImage)
    {
        if (frontsImages.Count > 0)
        {
            foreach (GameObject obj in frontsImages)
            {
                Destroy(obj);
            }
            frontsImages.Clear();
        }

        AddFrontImage(frontImage);
    }

    private void AddFrontImage(GameObject frontObjectPrefab)
    {
        GameObject newFront = null;
        newFront = Instantiate(frontObjectPrefab);

        newFront.transform.parent = backgroundParent.transform;
        if (frontsImages.Count == 0)
        {
            newFront.transform.localPosition = new Vector2(0, frontYShift);
        }
        else
        {
            GameObject lastObject = frontsImages[frontsImages.Count - 1];
            float frontSize = DefineObjBoundaries(lastObject);
            Vector2 backPos = new Vector2(frontSize, frontYShift);
            newFront.transform.localPosition = backPos;
        }

        frontsImages.Add(newFront);
    }
    
    private void SetBackImage(GameObject backObjectPrefab)
    {
        if (backImage != null)
        {
            Destroy(backImage);
        }

        GameObject newBack = Instantiate(backObjectPrefab);
        newBack.transform.parent = backgroundParent.transform;
        newBack.transform.localPosition = new Vector2(0, 0);
        backImage = newBack;
    }
    
    private float DefineObjBoundaries(GameObject obj)
    {
        float result = obj.GetComponent<Collider2D>().bounds.size.x;
        return result;
    }

    public void SetZeroSpeed()
    {
        this.frontMoveSpeed = 0.0f;
    }

    public void ReturnDefaultSpeed()
    {
        this.frontMoveSpeed = controller.GetGameSpeed();
    }

    public void SetBossBackground()
    {
        isBoss = true;
        SetBackImage(curBiomeHolder.GetBossBackgroundBack());
        SetFrontImage(curBiomeHolder.GetBossBackgroundFront());
    }
}
