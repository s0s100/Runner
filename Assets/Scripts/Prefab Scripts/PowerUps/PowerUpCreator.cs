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

        // Get parent if it exists
        if (this.transform.parent != null)
        {
            Vector3 parentPosition = transform.parent.position + transform.localPosition;
            GameObject newObject = Instantiate(generatedPowerUp, parentPosition, Quaternion.identity);
            newObject.transform.SetParent(transform.parent);
        } else
        {
            Instantiate(generatedPowerUp, transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }
}
