using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private GameObject generatedEnemyParent;
    private LevelGenerator levelGenerator;
    private GameController gameController;

    [SerializeField]
    private GameObject enemyObject;
    [SerializeField]
    private float enemyGenerationTime = 15.0f;
    [SerializeField]
    private float yEnemyMinDist = -2.0f;
    [SerializeField]
    private float yEnemyMaxDist = 4.0f;
    [SerializeField]
    private float currentEnemyGenerationTime = 15.0f;
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
        GenerateEnemy();
    }

    private void GenerateEnemy()
    {
        if (currentEnemyGenerationTime > 0)
        {
            currentEnemyGenerationTime -= Time.deltaTime;
        }
        else
        {
            currentEnemyGenerationTime = enemyGenerationTime;
            
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

            float yDist = Random.Range(yEnemyMinDist, yEnemyMaxDist);
            yDist += this.transform.position.y;

            Vector2 witchPos = new Vector2(xDist, yDist);
            GameObject newEnemy = Instantiate(enemyObject);

            // Reverse enemy if possible
            if (isReversed)
            {
                FlyingEnemy EnemyControl = newEnemy.GetComponent<FlyingEnemy>();
                EnemyControl.SetLeftToRightMovement();
                EnemyControl.SetSinMovement();
                newEnemy.transform.Rotate(0.0f, 180.0f, 0.0f); // Rotate to make her move in the other direction
            }

            newEnemy.transform.position = witchPos;
            newEnemy.transform.parent = generatedEnemyParent.transform;
        }
    }
}
