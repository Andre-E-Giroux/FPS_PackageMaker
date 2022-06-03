using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPlayerHUD_Controller : MonoBehaviour
{
    public Text currentAmmoText;
    public Text currentReserveAmmoText;

    public void SetCurrentMagazineAmmoText(int ammoCount)
    {
        currentAmmoText.text = ammoCount.ToString();
    }
    public void SetCurrentReserveAmmoText(int reserveAmmoCount)
    {
        currentReserveAmmoText.text = reserveAmmoCount.ToString();
    }

}
