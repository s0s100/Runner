using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchControl : MonoBehaviour
{
    private GameController gameController;
    private float speed;
    private bool leftToRightMovement = false;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        speed = gameController.moveSpeeed / 2; // Half speed of the player
    }

    public void setLeftToRightMovement(bool newVal)
    {
        leftToRightMovement = newVal;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speedIncrement;
        if (leftToRightMovement)
        {
            speedIncrement = Vector3.right * (gameController.moveSpeeed + speed) * Time.deltaTime;
            this.transform.position += speedIncrement;
        } else
        {
            speedIncrement = (Vector3.left * (speed)) * Time.deltaTime;
            this.transform.position += speedIncrement;
        }
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
