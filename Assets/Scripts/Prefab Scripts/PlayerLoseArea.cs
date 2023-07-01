using UnityEngine;

public class PlayerLoseArea : MonoBehaviour
{
    public GameController gameController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objectTag = collision.gameObject.tag;
        if (objectTag == "Player")
        {
            gameController.GameDefeat();
        }
    }
}
