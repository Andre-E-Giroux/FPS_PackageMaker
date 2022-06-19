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
    private ObjectPooler bulletHoleDecalPooler;

    private void Awake()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if(!bulletHoleDecalPooler)
        {
            GameObject[] hold = GameObject.FindGameObjectsWithTag("Pool");
            for(int i = 0; i < hold.Length; i++)
            {
                if(hold[i].GetComponent<ObjectPooler>().poolerID == "pool_BulletHole")
                {
                    bulletHoleDecalPooler = hold[i].GetComponent<ObjectPooler>();
                }
            }
        }
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
                Debug.Log("hit point: " + hit.point);

                hitEntity.AddHealth(-WEAPON_DAMAGE);
            }
            else
            {
                Debug.Log("Decal!");
                GameObject bulletHole = bulletHoleDecalPooler.GetPooledObject();
                Debug.Log("hit point: " + hit.point);
                bulletHole.transform.position = hit.point;
                bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                bulletHole.transform.parent = hit.transform;
                bulletHole.SetActive(true);
            }

        }
        //weapon decal
        
    }
    public override void Fire2( )
    {
        base.Fire2();
    }




}
