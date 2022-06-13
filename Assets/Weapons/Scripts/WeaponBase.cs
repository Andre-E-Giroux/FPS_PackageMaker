using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    /// <summary>
    /// Weapon Range in meters
    /// </summary>
    [SerializeField]
    protected float MAX_WEAPON_RANGE = 5;
    /// <summary>
    /// Fire rate seconds between shots
    /// </summary>
    [SerializeField]
    protected float WEAPON_FIRE_RATE = 2;
    /// <summary>
    /// Weapon switch time, measured in seconds
    /// </summary>
    [SerializeField]
    protected float WEAPON_SWITCH_TIME = 1;
    /// <summary>
    /// Effect on player speed
    /// </summary>
    [SerializeField]
    protected float PLAYER_SPEED_EFFECT = 0;
    /// <summary>
    /// Maximum Number of "bullets" in the weapon
    /// </summary>
    [SerializeField] 
    protected int MAX_MAGAZINE_SIZE = 30;
    /// <summary>
    /// Maximum number of "bullets" in reserve
    /// </summary>
    [SerializeField] 
    protected int MAX_RESERVE_AMMUNITION = 240;
    /// <summary>
    /// Reload weapon speed, measured in seconds
    /// </summary>
    [SerializeField] 
    protected float RELOAD_SPEED = 1;
    /// <summary>
    /// Minimum Size of the cone that projectiles can fire within, 0 = NO CONE - PIN POINT ACCURACY
    /// QUATERNION
    /// </summary>
    [SerializeField] 
    protected float MINIMUM_CONE_ACCURACY_SIZE = 0;
    /// <summary>
    /// Maximum Size of the cone that projectiles can fire within, 0 = NO CONE - PIN POINT ACCURACY
    /// QUATERNION
    /// </summary>
    [SerializeField]
    protected float MAXIMUM_CONE_ACCURACY_SIZE = 0;

    /// <summary>
    /// Minimum Size of the cone that projectiles can fire within, 0 = NO CONE - PIN POINT ACCURACY
    /// QUATERNION
    /// </summary>
    [SerializeField]
    protected float currentConeAccuracySize = 0;

    /// <summary>
    /// Size of the cone that projectiles can fire within
    /// </summary>
    [SerializeField]
    protected float NUMBER_OF_PROJECTILES_PER_SHOT = 1;
    /// <summary>
    /// Layer mask's for available taget's (Currently only entities)
    /// </summary>
    [SerializeField]
    protected LayerMask entityLayerMask;

    protected float nextFire = 0;

    [SerializeField]
    protected int currentMagazineAmmo = 20;

    [SerializeField]
    protected int currentReserveAmmo = 40;

    /// <summary>
    /// Possible Weapon types
    /// </summary>
    protected enum WeaponTypes
    {
        MEELEE,
        SIDEARM,
        RIFLE,
        AUTOMATIC_RIFLE,
        SUB_MACHINGUN,
        MACHINGUN,
        SPECIAL
    }

    /// <summary>
    /// The weapon's type
    /// </summary>
    [SerializeField]
    protected WeaponTypes weaponType;

    /// <summary>
    /// Possible fire types
    /// </summary>
    protected enum FireTypes
    {
        MANUAL,
        SEMI_AUTO,
        BURST,
        AUTOMATIC
    }

    /// <summary>
    /// The weapon's fire type
    /// </summary>
    [SerializeField]
    protected FireTypes fireType;

    /// <summary>
    /// Hud controller
    /// </summary>
    private WeaponInteraction weaponInteraction;


    /// <summary>
    /// Primary Fire of weapon
    /// </summary>
    /// <returns>True if shot succesful</returns>
    public virtual bool Fire1() 
    {
        if(weaponInteraction)
        {
            weaponInteraction.UpdateHud();
        }
        return false; 
    }

    /// <summary>
    /// Secondary Fire of weapon
    /// </summary>
    public virtual void Fire2() { }

    public virtual void Reload()
    {
        Debug.Log("Reload, Weapon Base");
        if (MAX_MAGAZINE_SIZE > currentMagazineAmmo && currentReserveAmmo > 0)
        {
            if(MAX_MAGAZINE_SIZE >= currentReserveAmmo)
            {
                currentMagazineAmmo = currentReserveAmmo;
                currentReserveAmmo = 0;
            }
            else
            {
                currentReserveAmmo -= (MAX_MAGAZINE_SIZE - currentMagazineAmmo);
                currentMagazineAmmo = MAX_MAGAZINE_SIZE;
                
            }
        }
        weaponInteraction.UpdateHud();
    }



    protected void AwakenWeapon()
    {
        //entityLayerMask = LayerMask.NameToLayer("Entity");

        currentMagazineAmmo = MAX_MAGAZINE_SIZE;
        currentReserveAmmo = MAX_RESERVE_AMMUNITION;
        currentConeAccuracySize = MINIMUM_CONE_ACCURACY_SIZE;
    }


    public void AddInteractionManager(WeaponInteraction hud)
    {
        weaponInteraction = hud;
    }

    public int GetReserveAmmo()
    {
        return currentReserveAmmo;
    }

    public int GetMagazineAmmo()
    {
        return currentMagazineAmmo;
    }

    protected virtual Vector3 PickFiringDirection(Vector3 muzzleForward)
    {
        Vector3 candidate = Random.insideUnitSphere * currentConeAccuracySize + muzzleForward;
        return candidate.normalized;
    }

}
