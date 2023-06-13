using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollecting : MonoBehaviour
{
    [SerializeField]
    private int quantity = 1;

    [SerializeField]
    private AudioClip collectionSound;

    private CoinController coinController;
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
            coinController.AddCoins(quantity);
            AudioController.instance.PlayEffect(collectionSound, transform.position);
        }
    }

    private void ExterimateCoin()
    {
        Destroy(this.gameObject);
    }
}
