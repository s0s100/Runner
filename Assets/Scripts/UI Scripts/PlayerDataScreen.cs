using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Displays player data on the main screen
public class PlayerDataScreen : MonoBehaviour
{
    private const string BOSS_HEALTH_TEXT = "Boss health";

    [SerializeField]
    private Image weaponIcon;
    [SerializeField]
    private Image ammoBar;
    [SerializeField]
    private Sprite emptyHand;
    [SerializeField]
    private Image scoreBar;
    [SerializeField]
    private TMP_Text barText;

    // Health info
    private int curHealth;
    private int maxHealth;

    [SerializeField]
    private Image[] hearthImages;

    // Boss state transition
    private float curTransitionTime = 0.0f;
    private float transitionTime = 1.0f;
    private Color bossStageColor = new Color(0.7f, 0.3f, 0.3f, 1.0f);

    private void Update()
    {
        if (curTransitionTime > 0.0f)
        {
            scoreBar.color = Color.Lerp(scoreBar.color, bossStageColor, curTransitionTime);
            curTransitionTime -= Time.deltaTime;
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;

        // Remove unused hearths
        if (hearthImages.Length > maxHealth)
        {
            for (int i = maxHealth; i < hearthImages.Length; i++)
            {
                hearthImages[i].enabled = false;
            }
        }
    }

    public void SetCurHealth(int curHealth)
    {
        this.curHealth = curHealth;

        if (curHealth > maxHealth)
        {
            throw new System.Exception("Cur health > max health is not possible");
        }

        // Set empty hearths not in use
        for (int i = curHealth; i < maxHealth; i++)
        {
            Animator animator = hearthImages[i].GetComponent<Animator>();
            animator.SetTrigger("DirectLose");
        }
    }

    public void GetDamage()
    {
        curHealth--;

        Animator animator = hearthImages[curHealth].GetComponent<Animator>();
        animator.SetTrigger("LoseHearth");
    }

    public void GetHeart()
    {
        if (curHealth != maxHealth)
        {
            curHealth++;

            Animator animator = hearthImages[curHealth].GetComponent<Animator>();
            animator.SetTrigger("RestoreHearth");
        }
    }

    public void FillAmmoBar()
    {
        ammoBar.fillAmount = 1.0f;
    }

    public void SetAmmoCounter(float curAmmo, float maxAmmo)
    {
        float difference = curAmmo / maxAmmo;
        float result = 1.0f - difference;
        ammoBar.fillAmount = result;
    }

    public void UpdateWeaponImage(Sprite weaponImage)
    {
        weaponIcon.sprite = weaponImage;
    }

    public void SetEmptyHandIcon()
    {
        if (weaponIcon != null)
        {
            weaponIcon.sprite = emptyHand;
        }
    }

    public void UpdateScoreBar(int curScore, int maxScore)
    {
        // Useless check after
        if (maxScore != 0)
        {
            float result = (float)curScore / (float)maxScore;
            scoreBar.fillAmount = result;
        }
    }

    public void UpdateScoreBar(float proportion)
    {
        scoreBar.fillAmount = proportion;
    }

    // Used to display boss health
    public void SetToBossState()
    {
        curTransitionTime = transitionTime;
        barText.text = BOSS_HEALTH_TEXT;
    }
}
