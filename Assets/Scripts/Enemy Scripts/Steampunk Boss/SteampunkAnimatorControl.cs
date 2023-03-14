using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkAnimatorControl : MonoBehaviour
{
    // Lazer can't see parent object scripts
    [SerializeField]
    private LazerGenerator lazerGenerator;

    [SerializeField]
    private GameObject handAttackZone;

    [SerializeField]
    private EdgeCollider2D collider2D;

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

    public void FinishCurrentLevel()
    {
        Debug.Log("Congrats! You won!");
    }

    // Called upon boss death
    public void DisableColliderDamage()
    {
        collider2D.enabled = false;
        handAttackZone.SetActive(false);
    }
}