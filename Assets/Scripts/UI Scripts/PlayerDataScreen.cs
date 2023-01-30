using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Displays player data on the main screen
public class PlayerDataScreen : MonoBehaviour
{
    // Weapon info
    private float ammoPercentage; // Between 0 and 1

    [SerializeField]
    private Image weaponIcon;
    [SerializeField]
    private Image ammoBar;
    [SerializeField]
    private Sprite emptyHand;

    // Health info
    private int curHealth;
    private int maxHealth;

    [SerializeField]
    private Image[] hearthImages;

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
            animator.SetTrigger("LoseHearth");
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

    public void SetAmmoCounter(int curAmmo, int maxAmmo)
    {
        float result = (float)curAmmo / (float)maxAmmo;
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
}
