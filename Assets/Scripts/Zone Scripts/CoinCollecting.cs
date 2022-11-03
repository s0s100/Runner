using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollecting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        string objectTag = collidedObject.tag;
        if (objectTag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
