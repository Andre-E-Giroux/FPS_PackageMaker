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
    /// On fire, decrease weapon accuracy by adding this variable to currentConeAccuracySize.
    /// </summary>
    [SerializeField]
    protected float accuracyBloomIncrease = 0;

    /// <summary>
    /// Speed of how fast a weapon's accuracy recovers from a shot
    /// </summary>
    [SerializeField]
    protected float accuracyBloomDecreaseSpeed = 0;


    /// <summary>
    /// Size of the cone that projectiles can fire within
    /// </summary>
    [SerializeField]
    protected float NUMBER_OF_PROJECTILES_PER_SHOT = 1;
    /// <summary>
    /// Layer mask's for available target's 
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
    /// Weapon Reload animation name, set within script and weapon name. "anim_*nameOfWeapon*_Reload"
    /// </summary>
    private string weaponRelaodAnimationName;

    /// <summary>
    /// Weapon Switch animation name, set within script and weapon name. "anim_*nameOfWeapon*_Reload"
    /// </summary>
    private string weaponSwitchAnimationName;

    /// <summary>
    /// If the weapon can reload (Stop reload in middle of burst)
    /// </summary>
    protected bool allowedToReload = false;

    /// <summary>
    /// If false weapon will not fire, or reload, but can still switch
    /// If true weapon can do all basic functions unless another variable says other wise
    /// </summary>
    protected bool allowWeaponInteraction = true;

   

    [SerializeField]
    protected Animator weaponAnimator;


    /// <summary>
    /// Hud controller
    /// </summary>
    protected WeaponInteraction weaponInteraction;
    
    /// <summary>
    /// Player camera
    /// </summary>
    [SerializeField]
    protected Camera playerCamera;


    public float GetCurrentConeSize()
    {
        return currentConeAccuracySize;
    }

    [SerializeField]
    private float accuracyModifier = 1;

    /// <summary>
    /// Primary Fire of weapon, user commanded
    /// </summary>
    /// <param name="weaponSuper"> the child script that will call this script</param>
    /// <returns>True if shot succesful</returns>
    public virtual bool Fire1_Interaction(WeaponBase weaponSuper) 
    {
        if (!allowWeaponInteraction)
            return false;

        if (Time.time > WEAPON_FIRE_RATE + nextFire && currentMagazineAmmo > 0 && !isReloading)
        {
            for (int i = 0; i < NUMBER_OF_PROJECTILES_PER_SHOT; i++)
            {
                weaponSuper.Fire1();
                AccuracyDecrease();
            }

            --currentMagazineAmmo;

            nextFire = Time.time;

            //BASE
            // stop cycle reload, allowing player to chamber at least one round before allowing to fire (remember l4d shotgun reload)
            if (isReloading && isCycleReload)
            {
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

        return false;




    }

    /// <summary>
    /// Weapon Main fire function.
    /// HitScan weapons will use this function to shoot a raycast
    /// Projectile weapons will use thi function to spawn and launch a projectile(gameobject)
    /// </summary>
    public virtual void Fire1(){}



    /// <summary>
    /// Secondary Fire of weapon
    /// </summary>
    public virtual void Fire2() { }

    /// <summary>
    /// Base reload actions for weapon
    /// </summary>
    public virtual void Reload()
    {
        if (!allowWeaponInteraction)
            return;

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
    /// When weapon is activated
    /// </summary>
    public void EnableWeapon()
    {
        allowedToReload = true;
        WeaponReset();

        SetWeaponAnimationSpeed();
        if (!weaponAnimator)
            AllowWeaponInteraction();
    }

    /// <summary>
    /// Allow weapon to be interacted, not counting weapon switch, all functions disabled until this function is called
    /// </summary>
    public void AllowWeaponInteraction()
    {
        allowWeaponInteraction = true;
    }

    /// <summary>
    /// Weapon base awake function. Called from WeaponInteraction player script
    /// </summary>
    public void AwakenWeapon()
    {
        //entityLayerMask = LayerMask.NameToLayer("Entity");
        allowedToReload = true;

        weaponFire1AnimationName = "anim_" + nameOfWeapon + "_Fire1";
        weaponRelaodAnimationName = "anim_" + nameOfWeapon + "_Reload";
        weaponSwitchAnimationName = "anim_" + nameOfWeapon + "_Switch";

        currentMagazineAmmo = MAX_MAGAZINE_SIZE;
        currentReserveAmmo = MAX_RESERVE_AMMUNITION;
        currentConeAccuracySize = GetMinAccuracyModified();

        SetWeaponAnimationSpeed();
        
        // Ensure that at least 1 round is chambered if cycleLoadIsTrue
        if (isCycleReload && numberOfRoundsLoadedPerCycle <= 0)
            numberOfRoundsLoadedPerCycle = 1;

        // update hud
        weaponInteraction.UpdateHud();
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
        allowWeaponInteraction = false;
    }

  

    private void SetWeaponAnimationSpeed()
    {
        bool animationSwitchFound = false;

        // Get list of states in the animator
        UnityEditor.Animations.AnimatorController ac = weaponAnimator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
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
                   /// Debug.Log("Mod " + weaponFire1AnimationName + "animation speed");
                    ///Debug.Log("Clip length = " + clip.length + " Rload time = " + RELOAD_TIME);
                    weaponAnimator.SetFloat("animationSpeed_Fire1", clip.length / WEAPON_FIRE_RATE);
                }
            }
            //Is the state reload
            else if (state.name == weaponRelaodAnimationName)
            {
                AnimationClip clip = state.motion as AnimationClip;
                if (clip != null)
                {
                   // Debug.Log("Mod " + weaponFire1AnimationName + " animation speed");
                    //Debug.Log("Clip length = " + clip.length + " Rload time = " + RELOAD_TIME);
                    weaponAnimator.SetFloat("animationSpeed_Reload", clip.length / RELOAD_TIME);
                }
            }
            
            else if (state.name == weaponSwitchAnimationName)
            {
                AnimationClip clip = state.motion as AnimationClip;
                if (clip != null)
                {
                   // Debug.Log("Mod " + weaponSwitchAnimationName + " animation speed");
                  //  Debug.Log("Clip length = " + clip.length + " Switch time = " + WEAPON_SWITCH_TIME);
                    weaponAnimator.SetFloat("animationSpeed_Switch", clip.length / WEAPON_SWITCH_TIME);
                    animationSwitchFound = true;
                }
            }
            
        }

        if (!animationSwitchFound)
            AllowWeaponInteraction();
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

    /// <summary>
    /// Decrease accuracy by decrease accuracy by variable accuracyBloomIncrease
    /// </summary>
    protected void AccuracyDecrease()
    {
        if (currentConeAccuracySize < GetMaxAccuracyModified())
        {
            currentConeAccuracySize += accuracyBloomIncrease;
            currentConeAccuracySize = Mathf.Clamp(currentConeAccuracySize, GetMinAccuracyModified(), GetMaxAccuracyModified());
        }
    }

    /// <summary>
    /// Decrease bloom based on the variable accuracyBloomDecreaseSpeed overtime
    /// </summary>
    protected void UpdateAccuracy()
    {

        if (currentConeAccuracySize > GetMinAccuracyModified())
        {
            currentConeAccuracySize -= accuracyBloomDecreaseSpeed * Time.deltaTime;
            currentConeAccuracySize = Mathf.Clamp(currentConeAccuracySize, GetMinAccuracyModified(), GetMaxAccuracyModified());
        }
        else if (currentConeAccuracySize < GetMinAccuracyModified())
        {
            currentConeAccuracySize += accuracyBloomDecreaseSpeed * Time.deltaTime;
            currentConeAccuracySize = Mathf.Clamp(currentConeAccuracySize, currentConeAccuracySize, GetMaxAccuracyModified());
        }
    }


    private float GetMaxAccuracyModified()
    {
        return MAXIMUM_CONE_ACCURACY_SIZE * accuracyModifier;
    }

    private float GetMinAccuracyModified()
    {
        return MINIMUM_CONE_ACCURACY_SIZE * accuracyModifier;
    }

    /// <summary>
    /// DO NOT USE
    /// </summary>
    /// <returns></returns>
    public void SetCurrentAccuracyToMin()
    {
        currentConeAccuracySize = GetMinAccuracyModified();

    }

    private void Update()
    {
        UpdateAccuracy();
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



    public void UpdateWeaponFromPlayerState(float modifer)
    {
        accuracyModifier = modifer;
    }

}
