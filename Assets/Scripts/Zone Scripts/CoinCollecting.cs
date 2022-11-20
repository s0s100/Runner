using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollecting : MonoBehaviour
{
    private CoinController coinController;
    private const float TIME_BEFORE_DELETE = 0.5f;
    private Animator animator;

    private void Start()
    {
        coinController = FindObjectOfType<CoinController>();
        animator = this.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        string objectTag = collidedObject.tag;
        if (objectTag == "Player")
        {
            animator.SetTrigger("IsTaken");
            coinController.AddCoin();
            Destroy(this.gameObject, TIME_BEFORE_DELETE);
        }
    }
}
