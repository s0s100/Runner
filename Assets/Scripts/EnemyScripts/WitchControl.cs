using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchControl : MonoBehaviour
{
    private GameController gameController;
    private float speed;
    private bool leftToRightMovement = false;
    private float existanceTime = 10.0f;

    // Sin() function movement (cos() acceleration coz of derivative)
    // y = A*sin(xtC)
    private bool isSinMoving = true;
    private float sinMoveAmplitude = 2f;
    private float sinMoveTimeCoef = 2.0f;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        speed = gameController.moveSpeeed; // Same speed as a player
        Destroy(this.gameObject, existanceTime); // Delete after
    }

    public void SetLeftToRightMovement(bool newVal)
    {
        leftToRightMovement = newVal;
        speed = gameController.moveSpeeed * 2;
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
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
        if (collision.tag == "Player")
        {
            PlayerHealthScript playerHealthScript =
                collision.gameObject.GetComponent<PlayerHealthScript>();

            playerHealthScript.getDamage();
        }
    }
}
