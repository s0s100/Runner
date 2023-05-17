using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : PowerUp
{
    protected override void PowerUpUse(GameObject playerObject)
    {
        float duration = 10.0f; // For now

        PlayerController controller = playerObject.GetComponent<PlayerController>();
        controller.AllowDoubleJump(duration, GetImageSprite(), GetIndicatorColor());
        //PlayerHealthScript healthScript = playerObject.GetComponent<PlayerHealthScript>();
        //healthScript.GetHealth();
    }
}
