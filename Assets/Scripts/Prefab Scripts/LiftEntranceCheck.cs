using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftEntranceCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject liftEntrance;

    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Player")
        {
            liftEntrance.SetActive(true);
            gameController.SetLiftStop(this.transform.position.x);
        }
    }
}
