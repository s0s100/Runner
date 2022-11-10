using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthScript : MonoBehaviour
{
    private const int START_HEALTH = 3;
    private const float MIN_SPRITE_TRANSPARENCY = 0.5f;
    private const float TRANSPARENCY_CHANGE_INCREMENT = 5.0f;

    public TMP_Text healthText;

    public float invulnerabilityTime = 1.0f; 
    public int maxHealth = 10;
    public int curHealth = START_HEALTH;

    private GameController gameController;
    private SpriteRenderer playerSpriteRenderer;
    private float curInvulnerability = 0.0f;
    private bool isIncreasingTransparency = false;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        healthText.text = curHealth.ToString();

        if (curInvulnerability > 0.0f)
        {
            curInvulnerability -= Time.deltaTime;
            invulnerabilityDisplaying();
        }
    }

    // Removes one hearth
    public void getDamage()
    {
        if (curInvulnerability <= 0)
        {
            isIncreasingTransparency = false;
            curInvulnerability = invulnerabilityTime;
            curHealth--;

            if (curHealth == 0)
            {
                gameController.GameDefeat();
            }
        }
    }

    // Called if player is invulnerable
    private void invulnerabilityDisplaying()
    {
        Color currentColor = playerSpriteRenderer.color;

        if (curInvulnerability <= 0.0f)
        {
            currentColor.a = 1.0f;
        } else
        {
            if (isIncreasingTransparency)
            {
                currentColor.a += Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;

                if (currentColor.a >= 1)
                    isIncreasingTransparency = false;
            } else
            {
                currentColor.a -= Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;

                if (currentColor.a <= MIN_SPRITE_TRANSPARENCY)
                    isIncreasingTransparency = true;
            }
        }
        playerSpriteRenderer.color = currentColor;
    }
}
