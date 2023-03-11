using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Class for the projectile weapons (raycast) that with burst fire action
/// </summary>
public class WeaponProjectileBurst : WeaponProjectile
{
    /// <summary>
    /// Fire rate in between burst, does not change
    /// </summary>
    public float BURST_FIRERATE = 0.6f;


    /// <summary>
    /// number of bullets fired in each burst
    /// </summary>
    public int ROUNDS_PER_BURST = 3;

    /// <summary>
    /// Tracks when burst can happen
    /// </summary>
    protected float untilNextBurst = 0;

    /// <summary>
    /// Is burst allowed to fire
    /// </summary>
    private bool allowedToFire = true;

    /// <summary>
    /// number of rounds left in burst
    /// </summary>
    private int burstsRemaining = 0;


    private void Update()
    {
        if (burstsRemaining > 0 && (GetMagazineAmmo() > 0 || !hasLimitedAmmunition))
        {
            if (allowedToReload)
                allowedToReload = !allowedToReload;


            if (base.Fire1_Interaction(this))
            {
                Debug.Log("FIRE1 CALLED");
                --burstsRemaining;

                if(GetMagazineAmmo() <= 0 && hasLimitedAmmunition)
                {
                    burstsRemaining = 0;
                }

                // burst cleaned, or ran out of ammo
                if (burstsRemaining <= 0 || (currentMagazineAmmo <= 0 && hasLimitedAmmunition))
                {
                    untilNextBurst = Time.time;
                    allowedToFire = true;
                    allowedToReload = true;
                    burstsRemaining = 0;
                    nextFire = nextFire = Time.time;
                }

            }
        }

        UpdateAccuracy();
    }

    public override bool Fire1_Interaction(WeaponBase weaponSuper)
    {

        if (Time.time >= (BURST_FIRERATE + untilNextBurst) && allowedToFire && (currentMagazineAmmo > 0 || !hasLimitedAmmunition))
        {
            allowedToFire = false;
            allowedToReload = true;
            burstsRemaining = ROUNDS_PER_BURST;
        }
        return true;
    }

   



     
}

/// <summary>
/// Editor inspector modifications
/// </summary>
#if UNITY_EDITOR
[CustomEditor(typeof(WeaponProjectileBurst))]
public class WeaponProjectileBurst_Editor : WeaponProjectile_Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        WeaponProjectileBurst script = (WeaponProjectileBurst)target;

        base.OnInspectorGUI();

        // TEMP - TODO: FIND A BETTER WAY TO seperate the sections "\n" (background color change?)
        // NAME OF SCRIPT SECTION //
        GUILayout.Label("\n|WEAPON PROJECTILE BURST SECTION|");

        //weapon burst fire rate
        EditorGUILayout.PropertyField(serializedObject.FindProperty("BURST_FIRERATE"));

        //number of shots per bursts
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ROUNDS_PER_BURST"));
        
        serializedObject.ApplyModifiedProperties();
    }

}
#endif
