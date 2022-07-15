
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This class dictates the base attribute and functions of all weapons, from melee, projectile and hitscan
/// </summary>
public class WeaponBase : MonoBehaviour
{
    // IDENTIFICATION VARIABLS: START /////////////////////////////////////////////////////////
        /// <summary>
        /// Name of weapon. animation names must follow this pattern: "anim_*nameOfWeapon*_*action*"
        /// Example: "anim_Pistol_Fire"
        /// </summary>
        public string nameOfWeapon;
    // IDENTIFICATION VARIABLS: END /////////////////////////////////////////////////////////

   


    // FIRE  VARIABLS: START /////////////////////////////////////////////////////////
        /// <summary>
        /// Size of the cone that projectiles can fire within
        /// </summary>
        public int NUMBER_OF_PROJECTILES_PER_SHOT = 1;

        /// <summary>
        /// Fire rate seconds between shots
        /// </summary>
        public float WEAPON_FIRE_RATE = 2;

        /// <summary>
        /// Time until next fire
        /// </summary>
        protected float nextFire = 0;
    // FIRE RATE VARIABLS: END /////////////////////////////////////////////////////////


    // REFRENCES VARIABLS: START /////////////////////////////////////////////////////////
        /// <summary>
        /// The weapon's 
        /// </summary>
        public Animator weaponAnimator;

        /// <summary>
        /// Player camera
        /// </summary>
        public Camera playerCamera;

        /// <summary>
        /// Layer mask's for available target's 
        /// </summary>
        public LayerMask entityLayerMask;
        // REFRENCES VARIABLS: END /////////////////////////////////////////////////////////


    // ACCURACY VARIABLS: START /////////////////////////////////////////////////////////
        /// <summary>
        /// The modifier to the weapons accuracy (SUMMAR must be fleshed out)
        /// </summary>
        public float accuracyModifier = 1;

        /// <summary>
        /// Minimum Size of the cone that projectiles can fire within, 0 = NO CONE - PIN POINT ACCURACY
        /// QUATERNION
        /// </summary>
        public float MINIMUM_CONE_ACCURACY_SIZE = 0;
        /// <summary>
        /// Maximum Size of the cone that projectiles can fire within, 0 = NO CONE - PIN POINT ACCURACY
        /// QUATERNION
        /// </summary>
        public float MAXIMUM_CONE_ACCURACY_SIZE = 0;

        /// <summary>
        /// Minimum Size of the cone that projectiles can fire within, 0 = NO CONE - PIN POINT ACCURACY
        /// QUATERNION
        /// </summary>
        protected float currentConeAccuracySize = 0;

        /// <summary>
        /// On fire, decrease weapon accuracy by adding this variable to currentConeAccuracySize.
        /// </summary>
        public float accuracyBloomIncrease = 0;

        /// <summary>
        /// Speed of how fast a weapon's accuracy recovers from a shot
        /// </summary>
        public float accuracyBloomDecreaseSpeed = 0;
    // ACCURACY VARIABLS: END /////////////////////////////////////////////////////////


    //  AMMUNITION VARIABLES:  START ////////////////////////////////////////////////////////

    /// <summary>
    /// The current amount of ammunition in the magazine/chamber
    /// </summary>
    protected int currentMagazineAmmo = 20;

        /// <summary>
        /// the current amount of ammunition in reserver
        /// </summary>
        protected int currentReserveAmmo = 40;

        /// <summary>
        /// Maximum Number of "bullets" in the weapon
        /// </summary>
        public int MAX_MAGAZINE_SIZE = 30;
        /// <summary>
        /// Maximum number of "bullets" in reserve
        /// </summary>
        public int MAX_RESERVE_AMMUNITION = 240;
    //  AMMUNITION VARIABLES:  END ////////////////////////////////////////////////////////




    // ANIMATION VARIABLS: START /////////////////////////////////////////////////////////
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
    // ANIMATION VARIABLS: END /////////////////////////////////////////////////////////

    // RELOAD VARIABLS: START /////////////////////////////////////////////////////////

        /// <summary>
        /// Does the weapon have limited Ammunition
        /// </summary>
        public bool hasLimitedAmmunition = false;
        
        /// <summary>
        /// Reload weapon speed, measured in seconds
        /// </summary>
        public float RELOAD_TIME = 1;


        /// <summary>
        /// True if the weapon is reloading, false if not
        /// </summary>
        protected bool isReloading = false;

        /// <summary>
        /// (SEEK SUMMARY!!!)
        /// </summary>
        protected bool stopReloadingCycle = false;


        /// <summary>
        /// If the weapon can reload (Stop reload in middle of burst)
        /// </summary>
        protected bool allowedToReload = false;


        // CYCLE VARIABLES //

        /// <summary>
        /// If the weapons need to have a number of rounds inserted rather than a magazine
        /// </summary>
        public bool isCycleReload = false;


        /// <summary>
        /// Weapon's number of rounds cycled in cycled reload
        /// </summary>
        public int numberOfRoundsLoadedPerCycle = 1;
    // RELOAD VARIABLS: END /////////////////////////////////////////////////////////


    // WEAPON INTERACTION VARIABLS: START /////////////////////////////////////////////////////////
        /// <summary>
        /// If false weapon will not fire, or reload, but can still switch
        /// If true weapon can do all basic functions unless another variable says other wise
        /// </summary>
        protected bool allowWeaponInteraction = true;

        /// <summary>
        /// Hud controller
        /// </summary>
        protected WeaponInteraction weaponInteraction;
    // WEAPON INTERACTION VARIABLS: END /////////////////////////////////////////////////////////


    // MISCELLANEOUS VARIABLS: START /////////////////////////////////////////////////////////
        /// <summary>
        /// Weapon switch time between weapons, measured in seconds
        /// </summary>
        public float WEAPON_SWITCH_TIME = 1;
        /// <summary>
        /// Effect on player speed
        /// </summary>
        public float PLAYER_SPEED_EFFECT = 0;
    // MISCELLANEOUS VARIABLS: END /////////////////////////////////////////////////////////


    public float GetCurrentConeSize()
    {
        return currentConeAccuracySize;
    }


    


    /// <summary>
    /// Primary Fire of weapon, user commanded
    /// </summary>
    /// <param name="weaponSuper"> the child script that will call this script</param>
    /// <returns>True if shot succesful</returns>
    public virtual bool Fire1_Interaction(WeaponBase weaponSuper) 
    {
        if (!allowWeaponInteraction)
            return false;

        if (Time.time > WEAPON_FIRE_RATE + nextFire && (currentMagazineAmmo > 0 || !hasLimitedAmmunition) && !isReloading)
        {
            for (int i = 0; i < NUMBER_OF_PROJECTILES_PER_SHOT; i++)
            {
                weaponSuper.Fire1();
                AccuracyDecrease();
            }

            if(hasLimitedAmmunition)
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

        if (!allowWeaponInteraction || !hasLimitedAmmunition)
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

        if(!weaponAnimator)
            weaponAnimator = GetComponent<Animator>();

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

       // Debug.Log("Setting animation Speed of " + nameOfWeapon + " weapon.");

        // Get list of states in the animator
        UnityEditor.Animations.AnimatorController ac = weaponAnimator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        UnityEditor.Animations.AnimatorStateMachine sm = ac.layers[0].stateMachine;



        // for every state find marked states and modify speed
        for (int i = 0; i < sm.states.Length; i++)
        {
            // current state in array at index
            UnityEditor.Animations.AnimatorState state = sm.states[i].state;

           // Debug.Log("stateName "+ state.name + " weapon fire name: " + weaponFire1AnimationName);

            // is the state Fire1?
            if (state.name == weaponFire1AnimationName)
            {
               // Debug.Log("Fire1Animation with name!");
                AnimationClip clip = state.motion as AnimationClip;
                if (clip != null)
                {
                  //  Debug.Log("Mod " + weaponFire1AnimationName + "animation speed");
                    //Debug.Log("Clip length = " + clip.length + " Rload time = " + RELOAD_TIME);
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
#if UNITY_EDITOR
[CustomEditor(typeof(WeaponBase))]
public class WeaponBase_Editor : Editor
{
    public override void OnInspectorGUI()
    {

        WeaponBase script = (WeaponBase)target;


        // NAME OF SCRIPT SECTION //
        GUILayout.Label("|WEAPON BASE SECTION|");


        // IDENTIFICATION //
        GUILayout.Label("IDENTIFICATION");
        //name
        script.nameOfWeapon = EditorGUILayout.TextField("Weapon Name", script.nameOfWeapon);

        // TEMP - TODO: FIND A BETTER WAY TO seperate the sections "\n" (background color change?)
        //  REFERENCES///
        GUILayout.Label("\nREFERENCES");
        //animator
        script.weaponAnimator = EditorGUILayout.ObjectField("Weapon Animator", script.weaponAnimator, typeof(Animator), true) as Animator;

        // Camera
        script.playerCamera = EditorGUILayout.ObjectField("Player Camera", script.playerCamera, typeof(Camera), true) as Camera;

        //--------------VERIFY!!!!
        string[] layers = { "Entity" };
        // Entity Layer Mask of targets
        script.entityLayerMask = EditorGUILayout.MaskField("Entity Layer Mask", script.entityLayerMask, layers);



        // TEMP - TODO: FIND A BETTER WAY TO seperate the sections "\n" (background color change?)
        // MISCELLANEOUS//
        GUILayout.Label("\nMISCELLANEOUS");
        //weapon switch speed
        script.WEAPON_SWITCH_TIME = EditorGUILayout.FloatField("Weapon Switch Time", script.WEAPON_SWITCH_TIME);

        // weapon speed effect
        script.PLAYER_SPEED_EFFECT = EditorGUILayout.FloatField("Weapon Speed Effect", script.PLAYER_SPEED_EFFECT);



        // TEMP - TODO: FIND A BETTER WAY TO seperate the sections "\n" (background color change?)
        // FIRE//
        GUILayout.Label("\nFIRE");
        //weapon fire rate
        script.WEAPON_FIRE_RATE = EditorGUILayout.FloatField("Weapon Fire Rate", script.WEAPON_FIRE_RATE);

        // projectiles per shot
        script.NUMBER_OF_PROJECTILES_PER_SHOT = EditorGUILayout.IntField("Number of projectile per shot", script.NUMBER_OF_PROJECTILES_PER_SHOT);




        // TEMP - TODO: FIND A BETTER WAY TO seperate the sections "\n" (background color change?)
        // ACCURACY//
        GUILayout.Label("\nRELOAD");
        // draw checkbox for the bool
        script.hasLimitedAmmunition = EditorGUILayout.Toggle("Weapon has limited ammo", script.hasLimitedAmmunition);
        if (script.hasLimitedAmmunition) // if bool is true, show other fields
        {
            script.MAX_MAGAZINE_SIZE = EditorGUILayout.IntField("Max Magazine Ammo", script.MAX_MAGAZINE_SIZE);
            script.MAX_RESERVE_AMMUNITION = EditorGUILayout.IntField("Max Reserve Ammo", script.MAX_RESERVE_AMMUNITION);
            script.RELOAD_TIME = EditorGUILayout.FloatField("Reload Time", script.RELOAD_TIME);

            // is cycle reload
            script.isCycleReload = EditorGUILayout.Toggle("Is cycle reload", script.isCycleReload);
            if (script.isCycleReload)
            {
                // number of cycle per reload
                script.numberOfRoundsLoadedPerCycle = EditorGUILayout.IntField("Number of rounds per Cycle", script.numberOfRoundsLoadedPerCycle);
            }
        }


        // TEMP - TODO: FIND A BETTER WAY TO seperate the sections "\n" (background color change?)
        // ACCURACY
        GUILayout.Label("\nACCURACY");
        //Minimum Weapon Accuracy
        script.MINIMUM_CONE_ACCURACY_SIZE = EditorGUILayout.FloatField("Minimum Weapon Accuracy", script.MINIMUM_CONE_ACCURACY_SIZE);
        
        //Maximum Weapon Accuracy
        script.MAXIMUM_CONE_ACCURACY_SIZE = EditorGUILayout.FloatField("Maximum Weapon Accuracy", script.MAXIMUM_CONE_ACCURACY_SIZE);
        
        //Accuracy bloom increase
        script.accuracyBloomIncrease = EditorGUILayout.FloatField("Accuracy bloom increase on shot", script.accuracyBloomIncrease);
        
        //Accuracy bloom decrease
        script.accuracyBloomDecreaseSpeed = EditorGUILayout.FloatField("Accuracy bloom decrease speed", script.accuracyBloomDecreaseSpeed);

        // Accuracy modifier
        script.accuracyModifier = EditorGUILayout.FloatField("Accuracy modifier", script.accuracyModifier);
    }
}
#endif