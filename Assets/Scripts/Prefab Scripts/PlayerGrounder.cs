using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("PlayerGrounded called!");
        string tag = collision.gameObject.tag;
        if (tag == "Player")
        {
            Debug.Log("I am grounded!");
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.DisableControl();
        }
    }
}
