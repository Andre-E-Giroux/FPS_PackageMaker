using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player interaction with active weapon and weapon hot bar
/// </summary>
public class WeaponInteraction : MonoBehaviour
{

    public int selectedWeapon = 0;
    public int maxNumberOfWeapons = 5;
    public GameObject[] weapons;
    
    [HideInInspector]
    public WeaponBase[] weaponBases;

    public Transform weaponPositionPoint;


    public WeaponPlayerHUD_Controller weaponPlayerHUD_Controller;

    public Transform projectileSpawnPoint;

    public PlayerSM playerStateMachine;

    public bool allowWeaponInteraction = true;
    private void Awake()
    {
        weaponBases = new WeaponBase[weapons.Length];
        // initiate weapons
        for (int i = 0; i < weapons.Length; i++)
        {
            if (!weaponPlayerHUD_Controller)
                weaponPlayerHUD_Controller = GetComponent<WeaponPlayerHUD_Controller>();
            weaponBases[i] = weapons[i].GetComponent<WeaponBase>();
            weapons[i].transform.position = weaponPositionPoint.position;
            weaponBases[i].AddInteractionManager(this);
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);

        weaponPlayerHUD_Controller.Startup(maxNumberOfWeapons, selectedWeapon, this);


        UpdateHud();
    }

    private void Update()
    {

        if (!allowWeaponInteraction)
            return;

        //Fire1
        if(Input.GetMouseButton(0))
        {
            weaponBases[selectedWeapon].Fire1_Interaction(weaponBases[selectedWeapon]);
        }
        //Reload
        if(Input.GetKeyDown(KeyCode.R))
        {
            weaponBases[selectedWeapon].Reload();
        }


        SwitchWeapons();

    }

    /// <summary>
    /// Switch weapons
    /// </summary>
    private void SwitchWeapons()
    {
        int temp = selectedWeapon;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(maxNumberOfWeapons > 0)
                temp = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (maxNumberOfWeapons > 1)
                temp = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (maxNumberOfWeapons > 2)
                temp = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (maxNumberOfWeapons > 3)
                temp = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (maxNumberOfWeapons > 4)
                temp = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (maxNumberOfWeapons > 5)
                temp = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (maxNumberOfWeapons > 6)
                temp = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (maxNumberOfWeapons > 7)
                temp = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (maxNumberOfWeapons > 8)
                temp = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (maxNumberOfWeapons > 9)
                temp = 9;
        }

        if (temp != selectedWeapon)
        {
            weapons[selectedWeapon].SetActive(false);
            weapons[temp].SetActive(true);
            weapons[temp].GetComponent<WeaponBase>().EnableWeapon();
            selectedWeapon = temp;

           // Debug.Log("State's name: " + playerStateMachine.GetCurrentState().name);

            UpdateWeaponFromPlayerState(((Universal)playerStateMachine.GetCurrentState()).GetWeaponAccuracyModifer());
            weapons[temp].GetComponent<WeaponBase>().SetCurrentAccuracyToMin();

            UpdateHud();
        }
    }

    /// <summary>
    /// Update hud on the player
    /// </summary>
    public void UpdateHud()
    {
        weaponPlayerHUD_Controller.SetCurrentMagazineAmmoText(weaponBases[selectedWeapon].GetMagazineAmmo());
        weaponPlayerHUD_Controller.SetCurrentReserveAmmoText(weaponBases[selectedWeapon].GetReserveAmmo());
        weaponPlayerHUD_Controller.ChangeSeletedItem(selectedWeapon);
    }

    /// <summary>
    /// Update weapon attributes that are affected by the player's state
    /// </summary>
    /// <param name="modifer">modifer accuracy modifier</param>
    public void UpdateWeaponFromPlayerState(float modifer)
    {
        weapons[selectedWeapon].GetComponent<WeaponBase>().UpdateWeaponFromPlayerState(modifer);
    }
}
