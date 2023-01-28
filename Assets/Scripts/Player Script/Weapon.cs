using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectileObject;
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    private float shootingSpeed;
    [SerializeField]
    private float curReload;

    private LevelGenerator levelGenerator;
    private int curAmmo;

    // Start is called before the first frame update
    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        curAmmo = maxAmmo;
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

            if (curAmmo == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void CreateProjectile()
    {
        GameObject newProjectile = Instantiate(projectileObject);
        newProjectile.transform.position = transform.position;
        levelGenerator.SetProjectileParent(newProjectile);
    }
}
