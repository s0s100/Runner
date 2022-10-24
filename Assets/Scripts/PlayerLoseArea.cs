using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoseArea : MonoBehaviour
{
    private static string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objectTag = collision.gameObject.tag;
        if (objectTag == playerTag)
        {
            // 
        }
    }
}
