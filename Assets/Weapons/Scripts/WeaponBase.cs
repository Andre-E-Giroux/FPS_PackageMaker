using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    /// <summary>
    /// Name of weapon. animation names must follow this pattern: "anim_*nameOfWeapon*_*action*"
    /// Example: "anim_Pistol_Fire"
    /// </summary>
    public string nameOfWeapon;
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
    protected float RELOAD_TIME = 1;
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
    /// True if the weapon is reloading, false if not
    /// </summary>
    [SerializeField]
    protected bool isReloading = false;

    /// <summary>
    /// Weapon Fire 1 animation name, set within script and weapon name. "anim_*nameOfWeapon*_Fire1"
    /// </summary>
    private string weaponFire1AnimationName;
    /// <summary>
    /// Weapon Fire 1 animation name, set within script and weapon name. "anim_*nameOfWeapon*_Reload"
    /// </summary>
    private string weaponRelaodAnimationName;

    /// <summary>
    /// If the weapon can reload (Stop reload in middle of burst)
    /// </summary>
    protected bool allowedToReload = false;

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

    [SerializeField]
    protected Animator weaponAnimator;


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
        if (weaponAnimator)
        {
            weaponAnimator.SetTrigger("isShooting_Fire1");
        }

        if (weaponInteraction)
        {
            weaponInteraction.UpdateHud();
        }
        

        return false; 
    }
    public virtual void EndFire1()
    {
        if (weaponAnimator)
        {
           // weaponAnimator.SetBool("isShooting_Fire1", false);
        }
    }


    /// <summary>
    /// Secondary Fire of weapon
    /// </summary>
    public virtual void Fire2() { }

    /// <summary>
    /// Base reload actions for weapon
    /// </summary>
    public virtual void Reload()
    {
        if (MAX_MAGAZINE_SIZE > currentMagazineAmmo && currentReserveAmmo > 0 && allowedToReload)
        {        
            isReloading = true;
            //Invoke("StoppedReloading", RELOAD_TIME);

            if (weaponAnimator)
            {
                weaponAnimator.SetTrigger("reload");
            }

        }
    }


    
    public void AwakenWeapon()
    {
        //entityLayerMask = LayerMask.NameToLayer("Entity");
        allowedToReload = true;

        currentMagazineAmmo = MAX_MAGAZINE_SIZE;
        currentReserveAmmo = MAX_RESERVE_AMMUNITION;
        currentConeAccuracySize = MINIMUM_CONE_ACCURACY_SIZE;

        weaponFire1AnimationName = "anim_" + nameOfWeapon + "_Fire1";
        weaponRelaodAnimationName = "anim_" + nameOfWeapon + "_Reload";

        // Get list of states in the animator
        UnityEditor.Animations.AnimatorController ac =  weaponAnimator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        UnityEditor.Animations.AnimatorStateMachine sm = ac.layers[0].stateMachine;

        // for every state find marked states and modify speed
        for (int i = 0; i < sm.states.Length; i++)
        {
            // current state in array at index
            UnityEditor.Animations.AnimatorState state = sm.states[i].state;

            // is the state Fire1?
            if (state.name == weaponFire1AnimationName)
            {
                AnimationClip clip = state.motion as AnimationClip;
                if (clip != null)
                {
                    Debug.Log("Mod " + weaponFire1AnimationName + "animation speed");
                    Debug.Log("Clip length = " + clip.length + " Rload time = " + RELOAD_TIME);
                    weaponAnimator.SetFloat("animationSpeed_Fire1", clip.length / WEAPON_FIRE_RATE);
                }
            }
            //Is the state reload
            else if (state.name == weaponRelaodAnimationName)
            {
                AnimationClip clip = state.motion as AnimationClip;
                if (clip != null)
                {
                    Debug.Log("Mod " + weaponFire1AnimationName + " animation speed");
                    Debug.Log("Clip length = " + clip.length + " Rload time = " + RELOAD_TIME);
                    weaponAnimator.SetFloat("animationSpeed_Reload", clip.length/RELOAD_TIME);
                }
            }
        }
    }


    public void AddInteractionManager(WeaponInteraction interactor)
    {
        Debug.Log("Interaction!");
        Debug.Log("Interactor = " + interactor);
        weaponInteraction = interactor;
    }

    public int GetReserveAmmo()
    {
        return currentReserveAmmo;
    }

    public int GetMagazineAmmo()
    {
        return currentMagazineAmmo;
    }

    public void WeaponSwitched()
    {
        isReloading = false;
    }

    protected virtual Vector3 PickFiringDirection(Vector3 muzzleForward)
    {
        Vector3 candidate = Random.insideUnitSphere * currentConeAccuracySize + muzzleForward;
        return candidate.normalized;
    }

    private void Update()
    {
       if(Time.time > WEAPON_FIRE_RATE + nextFire && currentMagazineAmmo > 0)
       {
            EndFire1();
       }

    }


    private void StoppedReloading()
    {
        Debug.Log("Reload stopped");
        isReloading = false;

       
        if (MAX_MAGAZINE_SIZE >= currentReserveAmmo)
        {
            currentMagazineAmmo = currentReserveAmmo;
            currentReserveAmmo = 0;
        }
        else
        {
            currentReserveAmmo -= (MAX_MAGAZINE_SIZE - currentMagazineAmmo);
            currentMagazineAmmo = MAX_MAGAZINE_SIZE;

        }
        Debug.Log("Interaction = " + weaponInteraction);
        weaponInteraction.UpdateHud();

    }



}
