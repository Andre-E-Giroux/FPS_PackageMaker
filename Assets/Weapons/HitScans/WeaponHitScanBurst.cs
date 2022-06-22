using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitScanBurst : WeaponHitScan
{
    /// <summary>
    /// Fire rate in between burst, does not change
    /// </summary>
    [SerializeField]
    protected float BURST_FIRERATE = 0.6f;

    /// <summary>
    /// Tracks when burst can happen
    /// </summary>
    [SerializeField]
    protected float untilNextBurst = 0;


    /// <summary>
    /// number of bullets fired in each burst
    /// </summary>
    [SerializeField]
    protected int burstLength = 3;

    /// <summary>
    /// Is burst allowed to fire
    /// </summary>
    private bool allowedToFire = true;

    /// <summary>
    /// number of rounds left in burst
    /// </summary>
    private int burstsRemaining = 0;


    private void Update()
    {
        if (burstsRemaining > 0 && GetMagazineAmmo() > 0)
        {
            if (allowedToReload)
                allowedToReload = !allowedToReload;


            if (base.Fire1_Interaction(this))
            {
                --burstsRemaining;

                if(GetMagazineAmmo() <= 0)
                {
                    burstsRemaining = 0;
                }

                // burst cleaned, or ran out of ammo
                if (burstsRemaining <= 0 || currentMagazineAmmo <= 0)
                {
                    untilNextBurst = Time.time;
                    allowedToFire = true;
                    allowedToReload = true;
                    burstsRemaining = 0;
                    nextFire = nextFire = Time.time;
                }

            }
        }

        UpdateAccuracy();
    }

    public override bool Fire1_Interaction(WeaponBase weaponSuper)
    {

        if (Time.time >= (BURST_FIRERATE + untilNextBurst) && allowedToFire)
        {
            allowedToFire = false;
            allowedToReload = true;
            burstsRemaining = burstLength;
        }
        return true;
    }

   



     
}
