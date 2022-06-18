using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairBehaviour : MonoBehaviour
{

    [SerializeField]
    private RectTransform crosshair;

    [SerializeField]
    private PlayerSM playerSM;

    [SerializeField]
    private WeaponInteraction weaponInteraction;


    private void Update()
    {
        UpdateCrosshair();
    }


    private void UpdateCrosshair()
    {
        crosshair.sizeDelta = new Vector2(weaponInteraction.weaponBases[weaponInteraction.selectedWeapon].GetCurrentConeSize() * 1000,
                                            weaponInteraction.weaponBases[weaponInteraction.selectedWeapon].GetCurrentConeSize() * 1000);
    }


}
