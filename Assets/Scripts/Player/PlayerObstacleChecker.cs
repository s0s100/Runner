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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    string tag = collision.gameObject.tag;
    //    Debug.Log("Collision: " + tag);
    //    if (tag == "Obstacle")
    //    {
    //        playerMovement.playerObstacleCollision();
    //    } 
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        //Debug.Log("Collision: " + tag);
        if (tag == "Obstacle")
        {
            playerMovement.playerObstacleCollision();
        }
    }
}
