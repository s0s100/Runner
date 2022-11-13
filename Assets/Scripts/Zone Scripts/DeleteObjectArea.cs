using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Area script to free memory and delete objects touching an area
/// </summary>

public class DeleteObjectArea : MonoBehaviour
{
    private static string[] deleteTags = { "Ground", "Decoration", "Obstacle" , "Enemy" };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objectTag = collision.gameObject.tag;
        if (deleteTags.Contains(objectTag))
        {
            Destroy(collision.gameObject);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    string objectTag = collision.gameObject.tag;
    //    if (deleteTags.Contains(objectTag))
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //}
}
