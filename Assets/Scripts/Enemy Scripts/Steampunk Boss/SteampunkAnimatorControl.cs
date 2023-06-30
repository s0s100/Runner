using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SteampunkAnimatorControl : MonoBehaviour
{
    private const float DEFAULT_SOUND_VOLUME = 0.5f;

    [SerializeField]
    private AudioClip apperanceSound;
    [SerializeField]
    private AudioClip fistHitSound;
    [SerializeField]
    private AudioClip lazerSound;
    [SerializeField]
    private AudioClip damagedSound;
    [SerializeField]
    private AudioClip dethSound;

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
    private AudioSource audioSource;

    [SerializeField]
    private Wire[] wires;

    private void Start()
    {
        animator = GetComponent<Animator>();
        uiController = FindObjectOfType<UIController>();
        gameController = FindObjectOfType<GameController>();
        audioSource = GetComponent<AudioSource>();

        AppearanceSound();
    }

    private void AppearanceSound()
    {
        audioSource.volume = DEFAULT_SOUND_VOLUME;
        audioSource.loop = false;
        audioSource.clip = apperanceSound;
        audioSource.Play();
    }

    private void FistAttackSound()
    {
        audioSource.loop = false;
        audioSource.clip = fistHitSound;
        audioSource.Play();
    }

    private void ActivateLazerSound()
    {
        audioSource.loop = true;
        audioSource.clip = lazerSound;
        audioSource.Play();
    }

    private void DeactivateLazerSound()
    {
        audioSource.Stop();
    }

    private void ActivateDamagedSound()
    {
        audioSource.loop = false;
        audioSource.clip = damagedSound;
        audioSource.Play();
    }

    private void ActivateDethSound()
    {
        audioSource.loop = false;
        audioSource.clip = dethSound;
        audioSource.Play();
    }

    public void ActivateLazer()
    {
        lazerGenerator.Activate();
        ActivateLazerSound();
    }

    public void DeactivateLazer()
    {
        lazerGenerator.Deactivate();
        DeactivateLazerSound();
    }

    public void EnableFistAttack()
    {
        handAttackZone.SetActive(true);
        FistAttackSound();
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

        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.CreateDisappearingDiamond(SteampunkBoss.REWARD_AMOUNT);

        ActivateDethSound();
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
        ActivateDamagedSound();
    }

    public void Damage2Screen()
    {
        screenAnimator.SetTrigger("Damage2");
        ActivateDamagedSound();
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