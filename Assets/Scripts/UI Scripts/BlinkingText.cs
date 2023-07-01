using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    private const float MAX_OPACITY = 0.7f;
    private const float MIN_OPACITY = 0.1f;

    private TMP_Text textHolder;
    private float incSpeed = 0.4f;
    private float curOpacity = MIN_OPACITY;

    void Start()
    {
        textHolder = this.GetComponent<TMP_Text>();
    }

    void Update()
    {
        UpdateOpacity();
    }

    private void UpdateOpacity()
    {
        curOpacity += Time.deltaTime * incSpeed;
        if (curOpacity >= MAX_OPACITY || curOpacity <= MIN_OPACITY)
        {
            incSpeed = -incSpeed;
        }
        Color curColor = textHolder.color;
        curColor.a = curOpacity;
        textHolder.color = curColor;
    }

    public void DeleteObject()
    {
        Destroy(this.gameObject);
    }
}
