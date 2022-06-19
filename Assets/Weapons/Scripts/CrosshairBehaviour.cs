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
        // universal// find a better accurate depiction, currently 2500
        crosshair.sizeDelta = new Vector2(weaponInteraction.weaponBases[weaponInteraction.selectedWeapon].GetCurrentConeSize() * 2500,
                                            weaponInteraction.weaponBases[weaponInteraction.selectedWeapon].GetCurrentConeSize() * 2500);
    }


}
