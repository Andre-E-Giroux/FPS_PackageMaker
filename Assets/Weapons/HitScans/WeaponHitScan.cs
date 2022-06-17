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

    private void Awake()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        AwakenWeapon();
    }

    public override void Fire1( )
    {
        
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
    public override void Fire2( )
    {
        base.Fire2();
    }




}
