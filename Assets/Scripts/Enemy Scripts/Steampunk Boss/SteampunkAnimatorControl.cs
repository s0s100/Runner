using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkAnimatorControl : MonoBehaviour
{
    // Lazer can't see parent object scripts
    [SerializeField]
    private Animator screenAnimator;
    [SerializeField]
    private LazerGenerator lazerGenerator;
    [SerializeField]
    private GameObject handAttackZone;
    [SerializeField]
    private EdgeCollider2D bossDamageCollider;

    private Animator animator;

    private UIController uiController;
    private GameController gameController;

    [SerializeField]
    private Wire[] wires;

    private void Start()
    {
        animator = GetComponent<Animator>();
        uiController = FindObjectOfType<UIController>();
        gameController = FindObjectOfType<GameController>();
    }

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
        StartCoroutine(BlackScreening());
    }

    private IEnumerator BlackScreening()
    {
        // Notify event system as well
        AnalyticsController analyticsController = AnalyticsController.instance;
        if (analyticsController != null)
        {
            analyticsController.BossKilled();
        }

        float waitTime = uiController.IsBlackScreenInvisible(false);
        yield return new WaitForSeconds(waitTime);
        gameController.StartNextLevel();
    }

    public void BossDeath()
    {
        bossDamageCollider.enabled = false;
        handAttackZone.SetActive(false);
        UnattachWires();

        // Also add coins and coin animation
        CoinController coinController = FindObjectOfType<CoinController>();
        coinController.AddCoins(SteampunkBoss.REWARD_AMOUNT);

        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.CreateDisappearingDiamond();
    }

    public void DisableDamageAnimation()
    {
        animator.ResetTrigger("IsDamaged1");
        animator.ResetTrigger("IsDamaged2");
    }

    public void EnableScreen()
    {
        screenAnimator.SetBool("IsActive", true);
    }

    public void DisableScreen()
    {
        screenAnimator.SetBool("IsActive", false);
    }
    
    public void ActivateHandAttackScreen ()
    {
        screenAnimator.SetTrigger("HandAttack");
    }

    public void Damage1Screen()
    {
        screenAnimator.SetTrigger("Damage1");
    }

    public void Damage2Screen()
    {
        screenAnimator.SetTrigger("Damage2");
    }

    public void FireBombingScreen()
    {
        screenAnimator.SetTrigger("FireBombing");
    }

    public void DeathScreen()
    {
        screenAnimator.SetTrigger("Death");
    }
        
    public void UnattachWires()
    {
        foreach (Wire wire in wires)
        {
            wire.UnattachAndDestroyWire();
        }
    }
}