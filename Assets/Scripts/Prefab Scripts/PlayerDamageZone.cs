using UnityEngine;

public class PlayerDamageZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealthScript playerHealthScript = 
                collision.gameObject.GetComponent<PlayerHealthScript>();

            playerHealthScript.GetDamage();
        }
    }
}
