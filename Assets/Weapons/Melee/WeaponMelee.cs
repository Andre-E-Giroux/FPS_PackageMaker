using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : WeaponBase
{

    private void Awake()
    {
        if (!playerCamera)
            playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }


    // Start is called before the first frame update
    void Start()
    {
        AwakenWeapon();
    }


    public override void Fire1()
    {
        Debug.Log("Weapon_Swing");
    }

}
