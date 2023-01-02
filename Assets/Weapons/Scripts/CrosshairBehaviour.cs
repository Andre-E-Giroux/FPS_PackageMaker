using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to keep track of weapon accuracy through hud crosshair (COME BACK)
/// </summary>
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

    /// <summary>
    /// update the crosshair on the hud to match firing accuracy of current weapon
    /// </summary>
    private void UpdateCrosshair()
    {
        // universal// find a better accurate depiction, currently 2500
        crosshair.sizeDelta = new Vector2(weaponInteraction.weaponBases[weaponInteraction.selectedWeapon].GetCurrentConeSize() * 2500,
                                            weaponInteraction.weaponBases[weaponInteraction.selectedWeapon].GetCurrentConeSize() * 2500);
    }


}
