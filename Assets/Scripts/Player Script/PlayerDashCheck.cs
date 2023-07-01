using UnityEngine;

public class PlayerDashCheck : MonoBehaviour
{
    private PlayerController playerMovement;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Obstacle")
        {
            playerMovement.DisableCurDash();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Ground" || tag == "Obstacle")
        {
            playerMovement.EnableCurDash();
        }
    }
}
