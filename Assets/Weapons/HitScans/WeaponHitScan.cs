using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeaponHitScan : WeaponBase
{
    /// <summary>
    /// raycastHIT for hitscan weapon
    /// </summary>
    protected RaycastHit hit;

    /// <summary>
    /// Reference to the pooler script object that handles the bullet hole sprites
    /// </summary>
    private ObjectPooler bulletHoleDecalPooler;


    /// <summary>
    /// DAMAGE THAT SINGLE WEAPON HIT WILL CAUSE
    /// </summary>
    public float WEAPON_DAMAGE = 10;


    /// <summary>
    /// Weapon Range in meters
    /// </summary>
    public float MAX_WEAPON_RANGE = 5;

    

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
        Debug.Log("FIRE1 called for hit scan for weapon: " + nameOfWeapon);
        
        Vector3 shotDirectionOffset = PickFiringDirection(Vector3.forward);

        Debug.DrawRay(playerCamera.transform.position, transform.TransformDirection(shotDirectionOffset) * MAX_WEAPON_RANGE, Color.yellow,30);

        if (Physics.Raycast(playerCamera.transform.position, transform.TransformDirection(shotDirectionOffset), out hit, MAX_WEAPON_RANGE, entityLayerMask))
        {
            Debug.Log( nameOfWeapon + "has hit a valid target");

            Entity hitEntity = hit.transform.transform.root.GetComponent<Entity>();
            if (hitEntity)
            {

                hitEntity.AddHealth(-WEAPON_DAMAGE);
            }
            else
            {
                GameObject bulletHole = bulletHoleDecalPooler.GetPooledObject();
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

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponHitScan))]
public class WeaponHitScan_Editor : WeaponBase_Editor
{
    public override void OnInspectorGUI()
    {
        WeaponHitScan script = (WeaponHitScan)target;

        base.OnInspectorGUI();

        // TEMP - TODO: FIND A BETTER WAY TO seperate the sections "\n" (background color change?)
        // NAME OF SCRIPT SECTION //
        GUILayout.Label("\n|WEAPON HITSCAN SECTION|");


        //weapon range
        script.MAX_WEAPON_RANGE = EditorGUILayout.FloatField("Max Weapon Range", script.MAX_WEAPON_RANGE);

        //weapon range
        script.WEAPON_DAMAGE = EditorGUILayout.FloatField("Weapon Damage", script.WEAPON_DAMAGE);

    }
}
#endif