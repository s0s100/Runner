using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomChecker : MonoBehaviour
{
    //private const float TIME_BETWEEN_FALL_PARTICLES = 0.5f;
    //private float curFallTimeParticles = 0.0f;

    private PlayerController playerMovement;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerController>();
    }

    //private void Update()
    //{
    //    if (curFallTimeParticles > 0.0f)
    //    {
    //        curFallTimeParticles -= Time.deltaTime;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Obstacle")
        {
            //if (curFallTimeParticles <= 0.0f)
            {
                playerMovement.CreateFallParticles();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Obstacle")
        {
            playerMovement.EnableJump();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Obstacle")
        {
            //curFallTimeParticles = TIME_BETWEEN_FALL_PARTICLES;
            playerMovement.DisableJump();
        }
    }
}
