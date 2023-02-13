using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomChecker : MonoBehaviour
{
    private PlayerController playerMovement;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Obstacle")
        {
            playerMovement.EnableJump();
            playerMovement.CreateFallParticles();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Obstacle")
        {
            playerMovement.DisableJump();
        }
    }
}
