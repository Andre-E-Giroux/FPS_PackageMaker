using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteraction : MonoBehaviour
{

    public int selectedWeapon = 0;
    public int MAX_NUMBER_OF_WEAPONS = 1;
    public GameObject[] weapons;
    [HideInInspector]
    public WeaponBase[] weaponBases;

    public Transform weaponPositionPoint;


    public WeaponPlayerHUD_Controller weaponPlayerHUD_Controller;

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


        
        UpdateHud();
    }

    private void Update()
    {
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

    private void SwitchWeapons()
    {
        int temp = selectedWeapon;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(MAX_NUMBER_OF_WEAPONS > 0)
                temp = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (MAX_NUMBER_OF_WEAPONS > 1)
                temp = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (MAX_NUMBER_OF_WEAPONS > 2)
                temp = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (MAX_NUMBER_OF_WEAPONS > 3)
                temp = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (MAX_NUMBER_OF_WEAPONS > 4)
                temp = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (MAX_NUMBER_OF_WEAPONS > 5)
                temp = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (MAX_NUMBER_OF_WEAPONS > 6)
                temp = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (MAX_NUMBER_OF_WEAPONS > 7)
                temp = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (MAX_NUMBER_OF_WEAPONS > 8)
                temp = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (MAX_NUMBER_OF_WEAPONS > 9)
                temp = 9;
        }

        if (temp != selectedWeapon)
        {
            weapons[selectedWeapon].SetActive(false);
            weapons[temp].SetActive(true);
            weapons[temp].GetComponent<WeaponBase>().EnableWeapon();
            selectedWeapon = temp;
            UpdateHud();
        }
    }


    public void UpdateHud()
    {
        weaponPlayerHUD_Controller.SetCurrentMagazineAmmoText(weaponBases[selectedWeapon].GetMagazineAmmo());
        weaponPlayerHUD_Controller.SetCurrentReserveAmmoText(weaponBases[selectedWeapon].GetReserveAmmo());
    }

}
