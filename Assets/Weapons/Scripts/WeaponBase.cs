using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{

    // Weapon Range in meters
    [SerializeField]
    protected const float MAX_WEAPON_RANGE = 5;
    //Fire rate in rounds per minute
    [SerializeField]
    protected const float FIRE_RATE = 2;
    // Weapon switch time, measured in seconds
    [SerializeField]
    protected const float SWITCH_WEAPON_TIME = 1;
    // Effect on player speed
    [SerializeField]
    protected const float SPEED_EFFECT = 0;
    //Maximum Number of "bullets" in the weapon
    [SerializeField] 
    protected const int MAX_MAGAZINE_SIZE = 30;
    //Maximum number of "bullets" in reserve
    [SerializeField] 
    protected const int MAX_RESERVE_AMMUNITION = 240;
    //Reload weapon speed, measured in seconds
    [SerializeField] 
    protected const float RELOAD_SPEED = 1;
    //Size of the cone that projectiles can fire within
    [SerializeField] 
    protected const float MINIMUM_CONE_SIZE = 0;
    //Size of the cone that projectiles can fire within
    [SerializeField]
    protected const float NUMBER_OF_PROJECTILES_PER_SHOT = 0;




    /// <summary>
    /// Primary Fire of weapon
    /// </summary>
    /// <param name="isBeingActivated">True: If the weapon's primary fire is being used or has been called to be used, false: if deactivated</param>
    protected abstract void Fire1(bool isBeingActivated);

    /// <summary>
    /// Secondary Fire of weapon
    /// </summary>
    /// <param name="isBeingActivated">True: If the weapon's secondary fire is being used or has been called to be used, false: if deactivated</param>
    protected abstract void Fire2(bool isBeingActivated);

}
