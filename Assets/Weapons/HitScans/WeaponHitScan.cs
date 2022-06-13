using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitScan : WeaponBase
{
    // damage to target based on HP
    [SerializeField]
    protected float WEAPON_DAMAGE = 10;

    //raycast for hitscan weapon
    protected RaycastHit hit;

    [SerializeField]
    protected Camera playerCamera;

    private void Awake()
    {
        AwakenWeapon();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public override bool Fire1( )
    {
        if (Time.time > WEAPON_FIRE_RATE + nextFire && currentMagazineAmmo > 0)
        {
            
            Debug.Log("Weapon fired!");

            for (int i = 0; i < NUMBER_OF_PROJECTILES_PER_SHOT; i++)
            {
                //Vector3 shotDirection = new Vector3(playerCamera.transform.rotation.x + Random.Range(-currentConeAccuracySize, currentConeAccuracySize), playerCamera.transform.rotation.y + Random.Range(-currentConeAccuracySize, currentConeAccuracySize), 10);

                Vector3 shotDirectionOffset = PickFiringDirection(Vector3.forward);

                Debug.DrawRay(playerCamera.transform.position, transform.TransformDirection(shotDirectionOffset) * MAX_WEAPON_RANGE, Color.yellow,30);

                if (Physics.Raycast(playerCamera.transform.position, transform.TransformDirection(shotDirectionOffset), out hit, MAX_WEAPON_RANGE, entityLayerMask))
                {

                    Debug.Log("Did Hit");

                    Entity hitEntity = hit.transform.transform.root.GetComponent<Entity>();
                    if (hitEntity)
                    {
                        Debug.Log("Damage!!!!");
                        hitEntity.AddHealth(-WEAPON_DAMAGE);
                    }
                }
            }

            --currentMagazineAmmo;

            nextFire = Time.time;

            base.Fire1();

            return true;
        }
        return false;
    }
    public override void Fire2( )
    {
        base.Fire2();
    }




}
