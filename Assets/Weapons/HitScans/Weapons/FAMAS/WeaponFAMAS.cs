using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFAMAS : WeaponHitScanBurst
{
    public override bool Fire1()
    {
        if (base.Fire1())
            return true;
        else
            return false;

    }
    public override void Fire2()
    {
        base.Fire2();
    }
}
