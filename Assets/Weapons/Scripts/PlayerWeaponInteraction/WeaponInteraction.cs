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


    private void Awake()
    {
        weaponBases = new WeaponBase[weapons.Length];
        // initiate weapons
        for (int i = 0; i < weapons.Length; i++)
        {
            weaponBases[i] = weapons[i].GetComponent<WeaponBase>();
            weapons[i].transform.position = weaponPositionPoint.position;
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            weaponBases[selectedWeapon].Fire1();
        }

        int temp = SwitchWeapons();
        
        if(temp != selectedWeapon)
        {
            weapons[selectedWeapon].SetActive(false);
            weapons[temp].SetActive(true);
            selectedWeapon = temp;
        }

    }

    private int SwitchWeapons()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(MAX_NUMBER_OF_WEAPONS > 0)
                return 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (MAX_NUMBER_OF_WEAPONS > 1)
                return 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (MAX_NUMBER_OF_WEAPONS > 2)
                return 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (MAX_NUMBER_OF_WEAPONS > 3)
                return 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (MAX_NUMBER_OF_WEAPONS > 4)
                return 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (MAX_NUMBER_OF_WEAPONS > 5)
                return 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (MAX_NUMBER_OF_WEAPONS > 6)
                return 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (MAX_NUMBER_OF_WEAPONS > 7)
                return 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (MAX_NUMBER_OF_WEAPONS > 8)
                return 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (MAX_NUMBER_OF_WEAPONS > 9)
                return 9;
        }

        return selectedWeapon;
    }


}
