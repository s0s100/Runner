using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpCreator : MonoBehaviour
{
    private const string POWER_UP_PATH = "Collectable/PowerUps";

    private void Awake()
    {
        GameObject[] powerUps = Resources.LoadAll(POWER_UP_PATH, typeof(GameObject)).Cast<GameObject>().ToArray();
        int selectedPowerUp = Random.Range(0, powerUps.Length);

        GameObject generatedPowerUp = powerUps[selectedPowerUp];
        generatedPowerUp.transform.position = gameObject.transform.position;
        Instantiate(generatedPowerUp);

        // FindObjectOfType<LevelGenerator>().SetGeneratedPrefabParent(generatedPowerUp);

        Destroy(this.gameObject);
    }
}
