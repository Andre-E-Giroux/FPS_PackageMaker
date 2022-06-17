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

    protected bool stopReloadingCycle = false;

    [SerializeField]
    protected bool isCycleReload = false;

    [SerializeField]
    protected int numberOfRoundsLoadedPerCycle = 1;


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
    
    [SerializeField]
    protected Camera playerCamera;




    /// <summary>
    /// Primary Fire of weapon
    /// </summary>
    /// <returns>True if shot succesful</returns>
    public virtual bool Fire1_Interaction(WeaponBase weaponSuper) 
    {


        if (Time.time > WEAPON_FIRE_RATE + nextFire && currentMagazineAmmo > 0 && !isReloading)
        {


            for (int i = 0; i < NUMBER_OF_PROJECTILES_PER_SHOT; i++)
            {
                weaponSuper.Fire1();
            }

            --currentMagazineAmmo;

            nextFire = Time.time;

            //BASE
            Debug.Log("Fire1 base : " + isReloading + " cycle " + isCycleReload);
            // stop cycle reload, allowing player to chamber at least one round before allowing to fire (remember l4d shotgun reload)
            if (isReloading && isCycleReload)
            {
                Debug.Log("Stop cycle reload");
                stopReloadingCycle = true;
            }

            else if (!isReloading)
            {
                if (weaponAnimator)
                {
                    weaponAnimator.SetTrigger("isShooting_Fire1");
                }


                if (weaponInteraction)
                {
                    weaponInteraction.UpdateHud();
                }
            }
            //BASE


            return true;
        }
        else if (isReloading)
        {
            stopReloadingCycle = true;
        }

        EndFire1();
        return false;




    }

    public virtual void Fire1()
    {
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
            else
            {
                Invoke("StoppedReloading", RELOAD_TIME);
            }

        }
    }


    /// <summary>
    /// Weapon base awake function. Called from WeaponInteraction player script
    /// </summary>
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

        // Ensure that at least 1 round is chambered if cycleLoadIsTrue
        if (isCycleReload && numberOfRoundsLoadedPerCycle <= 0)
            numberOfRoundsLoadedPerCycle = 1;
    }

    /// <summary>
    /// Get the player interaction manager. Called from player WeaponInteraction
    /// </summary>
    /// <param name="interactor"></param>
    public void AddInteractionManager(WeaponInteraction interactor)
    {
        weaponInteraction = interactor;
    }

    /// <summary>
    /// Get reserve ammunitioncaount
    /// </summary>
    /// <returns>Reserve ammo integer count</returns>
    public int GetReserveAmmo()
    {
        return currentReserveAmmo;
    }

    /// <summary>
    /// Get magazine ammunitioncaount
    /// </summary>
    /// <returns>Magazine ammo integer count</returns>
    public int GetMagazineAmmo()
    {
        return currentMagazineAmmo;
    }

    /// <summary>
    /// Function called when weapon is switched out. Reset certain elements of the weapon
    /// </summary>
    public void WeaponReset()
    {
        isReloading = false;
        isCycleReload = false;
    }

    /// <summary>
    /// Calculates direction that the shot will take, dependent on direction the camera is pointing and variable currentConeAccuracySize
    /// </summary>
    /// <param name="muzzleForward">Forward vector of the camera</param>
    /// <returns>Direction of the fire will take</returns>
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

    /// <summary>
    /// When time for the reload ends, this function is called. Either from reload animation End or if, no animatior, invoke calls this function after RELOAD_TIME seconds has passed
    /// </summary>
    private void StoppedReloading()
    {
        if (!isCycleReload)
        {
            isReloading = false;

            //Magazine need exceeds reserve ammo
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
        }
        else
        {
            //Magazine need exceeds reserve ammo
            if (numberOfRoundsLoadedPerCycle >= currentReserveAmmo)
            {
                currentMagazineAmmo += currentReserveAmmo;
                currentReserveAmmo = 0;
                //stop reload
                WeaponReset();

            }
            // ammo capped, reserve ammo remaining
            else if(currentMagazineAmmo + numberOfRoundsLoadedPerCycle >= MAX_MAGAZINE_SIZE)
            {
                // if loaded ammunition count exceed magazine size
                if(MAX_MAGAZINE_SIZE - currentMagazineAmmo < numberOfRoundsLoadedPerCycle)
                    currentReserveAmmo -= MAX_MAGAZINE_SIZE - currentMagazineAmmo;
                else
                    currentReserveAmmo -= numberOfRoundsLoadedPerCycle;

                currentMagazineAmmo = MAX_MAGAZINE_SIZE;

                //stop reload
                WeaponReset();
            }
            else
            {
                currentReserveAmmo -= numberOfRoundsLoadedPerCycle;
                currentMagazineAmmo += numberOfRoundsLoadedPerCycle;

                
            }
            if(stopReloadingCycle)
            {
                Debug.Log("Stop reloading");
                isReloading = false;
                stopReloadingCycle = false;
            }
            else
            {
                if (weaponAnimator)
                {
                    weaponAnimator.SetTrigger("reload");
                }
                else
                {
                    Invoke("StoppedReloading", RELOAD_TIME);
                }
            }

            
           
        }

        weaponInteraction.UpdateHud();

    }



}
