using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchGeneration : MonoBehaviour
{
    private GameObject generatedEnemyParent;
    private LevelGenerator levelGenerator;
    private GameController gameController;

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
    private float generationDistance;

    private void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        gameController = FindObjectOfType<GameController>();
        generationDistance = levelGenerator.GetGenerationDistance();
        generatedEnemyParent = levelGenerator.GetEnemyParent();

        if (!gameController.IsGameRunning())
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        GenerateWitch();
    }

    private void GenerateWitch()
    {
        if (currentWitchGenerationTime > 0)
        {
            currentWitchGenerationTime -= Time.deltaTime;
            // Debug.Log(currentWitchGenerationTime);
        }
        else
        {
            // Debug.Log("Witch is generated!");
            currentWitchGenerationTime = witchGenerationTime;

            // Should probably be a class value
            bool isReversed = Random.value > 0.5f; // Returns random bool
            bool isSinMoving = Random.value > 0.75f;

            float xDist;
            if (isReversed)
            {
                xDist = -generationDistance + this.transform.position.x;
            }
            else
            {
                xDist = generationDistance + this.transform.position.x;
            }

            float yDist = Random.Range(yWitchMinDist, yWitchMaxDist);
            yDist += this.transform.position.y;

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
}
