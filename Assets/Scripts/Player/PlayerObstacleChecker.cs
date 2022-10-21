using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObstacleChecker : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Obstacle")
        {
            playerMovement.playerObstacleCollision();
        }
    }
}
