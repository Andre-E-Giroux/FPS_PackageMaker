using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : WeaponBase
{

    /// <summary>
    /// Pooler of the chosen projectiles
    /// </summary>
    [SerializeField]
    private ObjectPooler projectilePool;
    

    private void Awake()
    {
        if(!playerCamera)
            playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (!projectilePool)
            projectilePool = GetComponent<ObjectPooler>();
    }


    // Start is called before the first frame update
    void Start()
    {
        AwakenWeapon();
    }

    public override void Fire1()
    {
        GameObject projectile = projectilePool.GetPooledObject();

        projectile.transform.position = weaponInteraction.projectileSpawnPoint.position;
        projectile.transform.rotation = weaponInteraction.projectileSpawnPoint.rotation;

        projectile.SetActive(true);
        //projectile.GetComponent<ProjectileScript>().FireProjectile();
    }
}
