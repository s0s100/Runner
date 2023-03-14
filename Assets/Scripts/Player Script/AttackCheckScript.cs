using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheckScript : MonoBehaviour
{
    private PlayerController playerController;
    private SpriteRenderer attackRenderer;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        attackRenderer = GetComponent<SpriteRenderer>();
        attackRenderer.color = playerController.GetAttackColor();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.tag;
        if (tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

           if (enemy.GetCurHealth() > 0 && playerController.CanAttack())
           {
               enemy.Damage();
               playerController.Attack();

               Animator animator = GetComponent<Animator>();
               animator.SetTrigger("IsAttacking");
           }
        }
    }
}
