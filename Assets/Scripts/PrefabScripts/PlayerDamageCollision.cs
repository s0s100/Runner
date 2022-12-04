using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCollision : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthScript playerHealthScript =
                collision.gameObject.GetComponent<PlayerHealthScript>();

            playerHealthScript.GetDamage();
        }
    }
}
