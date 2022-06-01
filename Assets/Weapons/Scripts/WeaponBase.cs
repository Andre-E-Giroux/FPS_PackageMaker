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
    protected float FIRE_RATE = 2;
    /// <summary>
    /// Weapon switch time, measured in seconds
    /// </summary>
    [SerializeField]
    protected float SWITCH_WEAPON_TIME = 1;
    /// <summary>
    /// Effect on player speed
    /// </summary>
    [SerializeField]
    protected float SPEED_EFFECT = 0;
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
    /// Size of the cone that projectiles can fire within
    /// </summary>
    [SerializeField] 
    protected float MINIMUM_CONE_SIZE = 0;
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
    /// Primary Fire of weapon
    /// </summary>
    /// <returns>True if shot succesful</returns>
    public virtual bool Fire1() { return false; }

    /// <summary>
    /// Secondary Fire of weapon
    /// </summary>
    public virtual void Fire2() { }

    private void Awake()
    {
        entityLayerMask = LayerMask.NameToLayer("Entity");
    }

}
