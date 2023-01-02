using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Class of the HitScan weapons (Raycasts)
/// </summary>
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


    public bool singleHitDamage = false;

    public float weaponInanimateImpactForce = 0f;


    private List<int> singleHitList = new List<int>();



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

    public override void AfterFire1() 
    {
        singleHitList.Clear();
    }

    /// <summary>
    /// Add physics force to object if it is a valid target
    /// </summary>
    /// <param name="rgHit">The rigidbody of the hit object</param>
    private void WeaponAddPhysicsForce(Rigidbody rgHit)
    {
        rgHit.AddForceAtPosition(((hit.point - transform.position).normalized) * (weaponInanimateImpactForce / hit.distance), hit.point, ForceMode.Impulse);
    }

    public override void Fire1(Vector3 shotDirection)
    {

      //  Debug.Log("FIRE1 called for hit scan for weapon: " + nameOfWeapon);
      //  Debug.Log("range: " + MAX_WEAPON_RANGE);

  
        Debug.DrawRay(playerCamera.transform.position, transform.TransformDirection(shotDirection) * MAX_WEAPON_RANGE, Color.yellow,30);

        if (Physics.Raycast(playerCamera.transform.position, transform.TransformDirection(shotDirection), out hit, MAX_WEAPON_RANGE, entityLayerMask))
        {
            Debug.Log( nameOfWeapon + "has hit a valid target:"+ hit.transform.gameObject.name + " with tag of: " + hit.transform.tag);

            Entity hitEntity = hit.transform.transform.root.GetComponent<Entity>();
            Rigidbody rgHit = hit.transform.GetComponent<Rigidbody>();

            if (hitEntity)
            {
                if (hitEntity.isAlive)
                {

                    if (singleHitDamage)
                    {
                        if (singleHitList.Contains(hitEntity.gameObject.GetInstanceID()))
                        {
                            return;
                        }
                        else
                            singleHitList.Add(hitEntity.gameObject.GetInstanceID());
                    }


                    Debug.Log(hitEntity.transform.gameObject.name);
                    //hitEntity.AddHealth(-WEAPON_DAMAGE);
                    if(hitEntity.TakeDamageBasedOnPart(WEAPON_DAMAGE, hit.distance, hit.transform.tag))
                    {
                        WeaponAddPhysicsForce(rgHit);
                    }
                }
                else
                {
                    if (rgHit)
                    {
                        WeaponAddPhysicsForce(rgHit);
                    }
                }


            }
            else
            {
                GameObject bulletHole = bulletHoleDecalPooler.GetPooledObject();
                bulletHole.transform.position = hit.point;
                bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                bulletHole.transform.parent = hit.transform;
                bulletHole.SetActive(true);


                if (rgHit)
                {
                    WeaponAddPhysicsForce(rgHit);
                }
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
        serializedObject.Update();


        //WeaponHitScan script = (WeaponHitScan)target;

        base.OnInspectorGUI();

        // TEMP - TODO: FIND A BETTER WAY TO seperate the sections "\n" (background color change?)
        // NAME OF SCRIPT SECTION //
        GUILayout.Label("\n|WEAPON HITSCAN SECTION|");


        //weapon range
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MAX_WEAPON_RANGE"));
        
        //weapon damage
        EditorGUILayout.PropertyField(serializedObject.FindProperty("WEAPON_DAMAGE"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("singleHitDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponInanimateImpactForce"));




        serializedObject.ApplyModifiedProperties();
    }
}
#endif