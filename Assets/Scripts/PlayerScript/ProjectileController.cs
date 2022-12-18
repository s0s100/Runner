using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private float speedModifier;
    [SerializeField]
    private float rotationSpeed;
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
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        transform.position += Vector3.right * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        int health = enemy.GetCurHealth();
        if (collision.tag == "Enemy" && health > 0)
        {
            enemy.Damage();
            Destroy(this.gameObject);
        }
    }
}