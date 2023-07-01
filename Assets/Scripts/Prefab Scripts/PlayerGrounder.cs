using UnityEngine;

public class PlayerGrounder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.DisableControl();
        }
    }
}
