using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchControl : MonoBehaviour
{
    private GameController gameController;
    private float speed;
    private bool leftToRightMovement = false;
    private float existanceTime = 10.0f;
    
    // Sin() function movement
    

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        speed = gameController.moveSpeeed; // Same speed as a player
        Destroy(this.gameObject, existanceTime); // Delete after
    }

    public void SetLeftToRightMovement(bool newVal)
    {
        leftToRightMovement = newVal;
    }
    
    void Update()
    {
        Vector3 speedIncrement;
        if (leftToRightMovement)
        {
            speedIncrement = Vector3.right * (gameController.moveSpeeed + speed) * Time.deltaTime; // Result speed equal to player speed
            this.transform.position += speedIncrement;
        } else
        {
            speedIncrement = (Vector3.left * (speed)) * Time.deltaTime; // Result speed equal to player speed x2
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
