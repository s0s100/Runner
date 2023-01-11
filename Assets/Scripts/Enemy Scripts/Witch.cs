using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : Enemy
{
    // Sin() function movement (cos() acceleration coz of derivative)
    // y = A*sin(xtC)
    private bool leftToRightMovement = false;
    private bool isSinMoving = false;
    private float sinMoveAmplitude = 2.0f;
    private float sinMoveTimeCoef = 2.0f;

    protected override void Awake()
    {
        base.Awake();
        speed = gameController.GetGameSpeed(); // Same speed as a player
    }

    public void SetSinMovement()
    {
        isSinMoving = true;
    }

    public void SetLeftToRightMovement()
    {
        leftToRightMovement = true;
        speed = gameController.GetGameSpeed() * 2;
    }

    protected override void Movement()
    {
        // X movement
        Vector3 speedIncrement;
        if (leftToRightMovement)
        {
            speedIncrement = Vector3.right * speed * Time.deltaTime; // Result speed equal to player speed
        }
        else
        {
            speedIncrement = (Vector3.left * speed) * Time.deltaTime; // Result speed equal to player speed x2

        }

        // Y movement
        if (isSinMoving)
        {
            // Depends on the X, the faster it moves, the less it depends on the x speed
            float sinCoef = this.transform.position.x * (sinMoveTimeCoef / speed);
            speedIncrement += Mathf.Cos(sinCoef) * sinMoveAmplitude * Vector3.up * Time.deltaTime;
        }


        this.transform.position += speedIncrement;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && health > 0)
        {
            PlayerHealthScript playerHealthScript =
                collision.gameObject.GetComponent<PlayerHealthScript>();

            playerHealthScript.GetDamage();
        }
    }

    protected override void Kill()
    {
        base.Kill();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
