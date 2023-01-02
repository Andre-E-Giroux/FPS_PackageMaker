using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPlayerHUD_Controller : MonoBehaviour
{
    // current weapon hud
    public Text currentAmmoText;
    public Text currentReserveAmmoText;

    //weapon list
    [SerializeField]
    private RectTransform listBackgroundRectTransform;

    [SerializeField]
    private RectTransform[] itemEntities;

    private int currentItemSelected;


    // 0 = unselected
    // 1 = selected
    [SerializeField]
    private Color[] boxColors = new Color[2];

    // 0 = unselected
    // 1 = selected
    [SerializeField]
    private Color[] iconColors = new Color[2];

    private WeaponInteraction weaponInteraction;

    // prefab
    [SerializeField]
    private GameObject itemBoxPrefab;

    // padding between hotbar boxes
    private float boxPadding = 10;

    /// <summary>
    /// Set the current magazine ammo count from the current weapon to the hud
    /// </summary>
    /// <param name="ammoCount">current ammo count</param>
    public void SetCurrentMagazineAmmoText(int ammoCount)
    {
        currentAmmoText.text = ammoCount.ToString();
    }

    /// <summary>
    /// Set the current reserve ammo count from the current weapon to the hud
    /// </summary>
    /// <param name="reserveAmmoCount">current reserve ammo count</param>
    public void SetCurrentReserveAmmoText(int reserveAmmoCount)
    {
        currentReserveAmmoText.text = reserveAmmoCount.ToString();
    }

    /// <summary>
    /// First function called when game starts
    /// </summary>
    /// <param name="numberOfItems">number of items in the hotbar</param>
    /// <param name="initialWeapon">the item that is the first to be active</param>
    /// <param name="wInteraction">weapon interaction object instance</param>
    public void Startup(int numberOfItems, int initialWeapon, WeaponInteraction wInteraction)
    {
       // Debug.Log("STARTUP");
        weaponInteraction = wInteraction;
        itemEntities = new RectTransform[numberOfItems];
        CreateItemListHud(numberOfItems);
        InitialWeapon(initialWeapon);

    }

    /// <summary>
    /// Set the initial weapon as the selected weapon in the hotbar hud
    /// </summary>
    /// <param name="initialWeapon"></param>
    public void InitialWeapon(int initialWeapon)
    {
        currentItemSelected =initialWeapon;
        ChangeItemHudColor(initialWeapon, 1);
    }

    /// <summary>
    /// Change the selected item in the hotbar
    /// </summary>
    /// <param name="itemSelected">Item that was selected</param>
    public void ChangeSeletedItem(int itemSelected)
    {
        ChangeItemHudColor(currentItemSelected, 0);
        currentItemSelected = itemSelected;
        ChangeItemHudColor(currentItemSelected, 1);
    }

    /// <summary>
    /// Change the color of the box and item icon color in the hud hot bar
    /// </summary>
    /// <param name="itemIndex">index of the item that is selected</param>
    /// <param name="colorTypeIndex">Index of the color that will be selected for the box and icon</param>
    private void ChangeItemHudColor(int itemIndex, int colorTypeIndex)
    {
        //box
        itemEntities[itemIndex].GetComponent<RawImage>().color = boxColors[colorTypeIndex];
        //icon
        itemEntities[itemIndex].GetChild(0).GetComponent<RawImage>().color = iconColors[colorTypeIndex];
    }


    /// <summary>
    /// Create the item list box on start up. Form up the bar, populate it with the correct number of boxes and add the icons to it.
    /// </summary>
    /// <param name="numberOfItems">Number of items in the list of weapons to be added to the hotbar</param>
    private void CreateItemListHud(int numberOfItems)
    {
        listBackgroundRectTransform.sizeDelta = new Vector2((itemBoxPrefab.GetComponent<RectTransform>().sizeDelta.x * numberOfItems) + ((numberOfItems + 1) * boxPadding) , itemBoxPrefab.GetComponent<RectTransform>().sizeDelta.y + boxPadding) ;
        
        for(int i = 0; i < numberOfItems; i++)
        {
            RectTransform newIcon = Instantiate(itemBoxPrefab).GetComponent<RectTransform>();
            newIcon.parent = listBackgroundRectTransform;
            newIcon.anchoredPosition = new Vector3(boxPadding + (boxPadding * i) + (i * itemBoxPrefab.GetComponent<RectTransform>().sizeDelta.x), 0, 0);
            Texture weaponIcon = weaponInteraction.weapons[i].GetComponent<WeaponBase>().weaponIcon;
            newIcon.GetChild(0).GetComponent<RawImage>().texture = weaponIcon;

            RectTransform newIconChildRect = newIcon.GetChild(0).GetComponent<RectTransform>();

            if (weaponIcon.width > weaponIcon.height)
            {
                newIconChildRect.sizeDelta = new Vector2(newIconChildRect.sizeDelta.x, (float)newIconChildRect.sizeDelta.y / ((float)weaponIcon.width / (float)weaponIcon.height));
            }
            else
            {
                //Debug.Log("Resize width");
                newIconChildRect.GetComponent<RectTransform>().sizeDelta = new Vector2((float)newIconChildRect.sizeDelta.x / ((float)weaponIcon.height/ (float)weaponIcon.width), newIconChildRect.sizeDelta.y);
            }

            itemEntities[i] = newIcon;

            ChangeItemHudColor(i, 0);
        }
    
    }

}
