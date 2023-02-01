using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectileObject;
    [SerializeField]
    private Sprite weaponSprite;
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    private float shootingSpeed;
    private float curReload;
    [SerializeField]
    private float spreadAngle;

    private LevelGenerator levelGenerator;
    private PlayerDataScreen playerDataScreen;
    private int curAmmo;

    // Start is called before the first frame update
    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        playerDataScreen = FindObjectOfType<PlayerDataScreen>();

        curAmmo = maxAmmo;
        playerDataScreen.FillAmmoBar();
        playerDataScreen.UpdateWeaponImage(weaponSprite);
    }

    private void OnDestroy()
    {
        playerDataScreen.SetEmptyHandIcon();
    }

    private void Update()
    {
        if (curReload > 0)
        {
            curReload -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (curReload <= 0)
        {
            CreateProjectile();
            curReload = shootingSpeed;
            curAmmo--;

            playerDataScreen.SetAmmoCounter(curAmmo, maxAmmo);

            if (curAmmo == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void CreateProjectile()
    {
        GameObject newProjectile = Instantiate(projectileObject);

        float randomizedAngle = Random.Range(-spreadAngle, spreadAngle);
        newProjectile.transform.position = transform.position;
        newProjectile.transform.Rotate(0, 0, randomizedAngle);

        levelGenerator.SetProjectileParent(newProjectile);
    }
}
