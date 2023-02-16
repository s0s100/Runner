using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheckScript : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        Debug.Log(tag);
        if (tag == "Enemy")
        {
            Debug.Log("Get it boi!");

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Damage();
                
            playerController.Attack();
        }
    }
}
