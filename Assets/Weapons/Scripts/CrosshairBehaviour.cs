using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairBehaviour : MonoBehaviour
{

    [SerializeField]
    private RectTransform crosshair;

    [SerializeField]
    private WeaponInteraction weaponInteraction;


    private void Update()
    {
        UpdateCrosshair();
    }


    private void UpdateCrosshair()
    {
        // universal
        crosshair.sizeDelta = new Vector2(weaponInteraction.weaponBases[weaponInteraction.selectedWeapon].GetCurrentConeSize() * 1000,
                                            weaponInteraction.weaponBases[weaponInteraction.selectedWeapon].GetCurrentConeSize() * 1000);
    }


}
