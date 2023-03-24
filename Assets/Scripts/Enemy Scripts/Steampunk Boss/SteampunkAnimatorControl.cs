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
    private EdgeCollider2D bossDamageCollider;

    private Animator animator;

    private UIController uiController;
    private GameController gameController;

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
        float waitTime = uiController.IsBlackScreenInvisible(false);
        yield return new WaitForSeconds(waitTime);
        gameController.StartNextLevel();
    }

    public void BossDeath()
    {
        bossDamageCollider.enabled = false;
        handAttackZone.SetActive(false);
    }

    public void DisableDamageAnimation()
    {
        animator.ResetTrigger("IsDamaged1");
        animator.ResetTrigger("IsDamaged2");
    }
}