using UnityEngine;
using UnityEngine.UI;

public class ImageVisabilityChanger : MonoBehaviour
{
    private Image image;
    public float alphaIncreaseSpeed = 0.5f;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (image.color.a < 1.0f)
        {
            Color newColor = image.color;
            newColor.a += alphaIncreaseSpeed * Time.deltaTime;
            image.color = newColor;
        }
        else
        {
            this.enabled = false;
        }
    }
}
