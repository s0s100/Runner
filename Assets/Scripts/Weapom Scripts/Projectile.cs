using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speedModifier;
    [SerializeField]
    private float deleteTime;

    private GameController gameController;
    private float speed;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        speed = speedModifier * gameController.GetGameSpeed();

        Destroy(this.gameObject, deleteTime);
    }

    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        transform.position += Vector3.right * Time.deltaTime * speed * Mathf.Cos(transform.rotation.z);
        transform.position += Vector3.up * Time.deltaTime * speed * Mathf.Sin(transform.rotation.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            int health = enemy.GetCurHealth();

            if (health > 0)
            {
                enemy.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
