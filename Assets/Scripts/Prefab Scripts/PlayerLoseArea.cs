using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoseArea : MonoBehaviour
{
    public GameController gameController;

    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objectTag = collision.gameObject.tag;
        if (objectTag == PLAYER_TAG)
        {
            gameController.GameDefeat();
        }
    }
}
