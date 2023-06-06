using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : PowerUp
{
    private const float DEFAULT_DURATION = 10.0f;
    private const float DURATION_INCREMENT = 5.0f;

    protected float duration = DEFAULT_DURATION;

    private void Awake()
    {
        GameController gameController = FindObjectOfType<GameController>();
        UpgradeData upgradeData = gameController.GetUpgradeData("Double Jump");
        int upgradeLevel = upgradeData.GetUpgradeStatus();
        
        duration = DEFAULT_DURATION + upgradeLevel * DURATION_INCREMENT;
    }

    protected override void PowerUpUse(GameObject playerObject)
    {
        PlayerController controller = playerObject.GetComponent<PlayerController>();
        controller.AllowDoubleJump(duration, GetImageSprite(), GetIndicatorColor());
    }
}
