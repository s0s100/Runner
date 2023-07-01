using UnityEngine;

public class RotationScript : MonoBehaviour
{
    private float rotationSpeed = 25.0f;

    // Start is called before the first frame update
    void Start()
    {
        bool isReverse = Random.value < 0.5;
        if (isReverse)
        {
            rotationSpeed = -rotationSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float rotation = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotation);
    }
}
