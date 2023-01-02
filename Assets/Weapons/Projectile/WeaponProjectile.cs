using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// Class used for projectil based weapons (enable object, place object in front of gun, fire)
/// </summary>
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

    public override void Fire1(Vector3 shotDirection)
    {
        GameObject projectile = projectilePool.GetPooledObject();
        Debug.DrawRay(playerCamera.transform.position, weaponInteraction.projectileSpawnPoint.TransformDirection(shotDirection) * 10, Color.yellow, 30);

        projectile.transform.position = weaponInteraction.projectileSpawnPoint.position;
        projectile.transform.forward = weaponInteraction.projectileSpawnPoint.TransformDirection(shotDirection);

        projectile.SetActive(true);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponProjectile))]
public class WeaponProjectile_Editor : WeaponBase_Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();
        // WeaponHitScanBurst script = (WeaponHitScanBurst)target;
        // NAME OF SCRIPT SECTION //
        //GUILayout.Label("WEAPON PROJECTILE SECTION");
        serializedObject.ApplyModifiedProperties();

    }
}
#endif