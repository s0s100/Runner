using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomChecker : MonoBehaviour
{
    private PlayerController playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerController>();
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
            playerMovement.DisableJump();
        }
    }
}
