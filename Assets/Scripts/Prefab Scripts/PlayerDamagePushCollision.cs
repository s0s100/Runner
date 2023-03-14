using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagePushCollision : MonoBehaviour
{
    [SerializeField]
    private float shiftDistance;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthScript playerHealthScript =
                collision.gameObject.GetComponent<PlayerHealthScript>();
            playerHealthScript.GetDamage();
            
            // Shift to the left
            PlayerController playerController =
                collision.gameObject.GetComponent<PlayerController>();
            playerController.MakeQuickShift(shiftDistance);
        }
    }
}
