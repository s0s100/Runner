using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : PowerUp
{
    protected override void PowerUpUse(GameObject playerObject)
    {
        PlayerHealthScript healthScript= playerObject.GetComponent<PlayerHealthScript>();
        healthScript.GetHealth();
    }
}
