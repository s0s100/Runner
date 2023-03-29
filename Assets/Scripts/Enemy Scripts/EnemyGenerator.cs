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
            CreateEnemy();            
        }
    }

    private void CreateEnemy()
    {
        bool isReversed = Random.value > 0.5f;

        float xDist;
        if (isReversed) xDist = -generationDistance + this.transform.position.x;
        else            xDist = generationDistance + this.transform.position.x;

        float yDist = Random.Range(yEnemyMinDist, yEnemyMaxDist);
        yDist += this.transform.position.y;

        Vector2 enemyPos = new Vector2(xDist, yDist);
        GameObject newEnemy = Instantiate(enemyObject, enemyPos, Quaternion.identity);

        // newEnemy.transform.position = enemyPos;
        newEnemy.transform.parent = generatedEnemyParent.transform;

        Debug.Log("Generated: " + enemyObject.name);
    }
}
