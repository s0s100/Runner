using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagePushTrigger : MonoBehaviour
{
    [SerializeField]
    private float xPushForce;
    [SerializeField]
    private float yPushForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthScript playerHealthScript =
                collision.gameObject.GetComponent<PlayerHealthScript>();
            playerHealthScript.GetDamage();

            PlayerController playerController =
                collision.gameObject.GetComponent<PlayerController>();
            playerController.PushPlayerBack(xPushForce, yPushForce);
        }
    }
}
