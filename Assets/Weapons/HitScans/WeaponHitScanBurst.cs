using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitScanBurst : WeaponHitScan
{
    [SerializeField]
    protected float BURST_FIRERATE = 0.6f;

    [SerializeField]
    protected float untilNextBurst = 0;


    /// <summary>
    /// number of bullets fired in each burst
    /// </summary>
    [SerializeField]
    protected int burstLength = 3;

    private bool allowedToFire = true;

    private int burstsRemaining = 0;


    private void Update()
    {
        if (burstsRemaining > 0)
        {
           
            Debug.Log("Burst fire allowed/continued");

            if (base.Fire1())
            {
                Debug.Log("One bullet of burst was fired!");
                --burstsRemaining;

                Debug.Log("Bursts remaining: " + burstsRemaining);
                // burst cleaned
                if (burstsRemaining <= 0)
                {
                    Debug.Log("Reset");
                    untilNextBurst = Time.time;
                    allowedToFire = true;
                    burstsRemaining = 0;
                    nextFire = nextFire = Time.time;
                }

            }
        }
    }

    public override bool Fire1()
    {

        if (Time.time >= (BURST_FIRERATE + untilNextBurst) && allowedToFire)
        {
            allowedToFire = false;
            burstsRemaining = burstLength;
        }
        return true;
    }

   



     
}
