using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkParentLazer : MonoBehaviour
{
    // Lazer can't see parent object scripts
    [SerializeField]
    private LazerGenerator lazerGenerator;

    [SerializeField]
    private GameObject handAttackZone;

    public void ActivateLazer()
    {
        lazerGenerator.Activate();
    }

    public void DeactivateLazer()
    {
        lazerGenerator.Deactivate();
    }

    public void EnableFistAttack()
    {
        handAttackZone.SetActive(true);
    }

    public void DisableFistAttack()
    {
        handAttackZone.SetActive(false);
    }
}
