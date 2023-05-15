using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float creationTime;
    private bool isDisappearing = false;
    private float disappearanceSpeed;

    [SerializeField]
    private float disappearanceTime = 1.0f;
    [SerializeField]
    private float disappearanceMoveSpeed = 2.0f;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private float sinMoveSpeed = 1.0f;
    [SerializeField]
    private float sinCoef = 0.1f;
    [SerializeField]
    private float timeCoef = 2.0f;
    [SerializeField]
    private float sinTimeShift = 0.0f;

    private void Awake()
    {
        creationTime = Time.time;
    }

    private void Update()
    {
        if (!isDisappearing)
        {
            VisualMovement();
        } else
        {
            Disappearance();
        }
        
    }

    private void Disappearance()
    {
        Vector3 movement = Vector3.up * Time.deltaTime * disappearanceMoveSpeed;
        gameObject.transform.position += movement;

        // Also make it disappear
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a -= disappearanceTime * Time.deltaTime;
        spriteRenderer.color = color;

        spriteRenderer = background.GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        color.a -= disappearanceTime * Time.deltaTime;
        spriteRenderer.color = color;

    }

    private void VisualMovement()
    {
        // Movement
        float curSpeed = Mathf.Sin((Time.time - creationTime) * timeCoef + ((sinTimeShift * Mathf.PI) / 180));
        Vector3 movement = Vector3.up * sinCoef * curSpeed * Time.deltaTime;
        gameObject.transform.position += movement;

        float colorVisability = (curSpeed + 1) / 2;
        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        Color newColor = spriteRenderer.color;
        newColor.a = colorVisability;
        spriteRenderer.color = newColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isDisappearing)
        {
            PowerUpUse(collision.gameObject);
            LateDestroy();
        }
    }

    private void LateDestroy()
    {
        Destroy(this.gameObject, disappearanceTime);
        disappearanceSpeed = 1 / disappearanceTime;
        isDisappearing = true;
    }

    protected virtual void PowerUpUse(GameObject playerObject)
    {
        Debug.Log("Virtual not implemented method");
    }
}
