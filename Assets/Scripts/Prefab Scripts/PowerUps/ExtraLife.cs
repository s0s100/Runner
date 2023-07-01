using UnityEngine;

public class ExtraLife : PowerUp
{
    private static int REWARD_AMOUNT = 5;

    protected override void PowerUpUse(GameObject playerObject)
    {
        PlayerHealthScript healthScript= playerObject.GetComponent<PlayerHealthScript>();

        if (healthScript.IsMaxHealth())
        {
            PlayerController playerController = playerObject.GetComponent<PlayerController>();
            playerController.CreateDisappearingDiamond(REWARD_AMOUNT);
        }
        else
        {
            healthScript.GetHealth();
        }
    }
}
