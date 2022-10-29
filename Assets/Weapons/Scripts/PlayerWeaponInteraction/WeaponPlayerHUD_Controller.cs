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


    public void SetCurrentMagazineAmmoText(int ammoCount)
    {
        currentAmmoText.text = ammoCount.ToString();
    }
    public void SetCurrentReserveAmmoText(int reserveAmmoCount)
    {
        currentReserveAmmoText.text = reserveAmmoCount.ToString();
    }


    public void Startup(int numberOfItems, int initialWeapon)
    {
        Debug.Log("STARTUP");
        itemEntities = new RectTransform[numberOfItems];
        CreateItemListHud(numberOfItems);
        InitialWeapon(initialWeapon);

    }

    public void InitialWeapon(int initialWeapon)
    {
        Debug.Log("initial weapon");

        currentItemSelected =initialWeapon;
        Debug.Log("Current item selected: "+currentItemSelected);
        ChangeItemHudColor(initialWeapon, 1);

    }

    public void ChangeSeletedItem(int itemSelected)
    {
        ChangeItemHudColor(currentItemSelected, 0);
        currentItemSelected = itemSelected;
        ChangeItemHudColor(currentItemSelected, 1);

        //Debug.Log("change selected item");

    }


    private void ChangeItemHudColor(int itemIndex, int colorTypeIndex)
    {
        Debug.Log("Change to colortype: " + colorTypeIndex);
        //box
        itemEntities[itemIndex].GetComponent<RawImage>().color = boxColors[colorTypeIndex];
        //icon
        itemEntities[itemIndex].GetChild(0).GetComponent<RawImage>().color = iconColors[colorTypeIndex];
    }


    // prefabs
    [SerializeField]
    private GameObject itemBoxPrefab;

    private float boxPadding = 10;
    private void CreateItemListHud(int numberOfItems)
    {
        //Debug.Log("create item list hud");
        listBackgroundRectTransform.sizeDelta = new Vector2((itemBoxPrefab.GetComponent<RectTransform>().sizeDelta.x * numberOfItems) + ((numberOfItems + 1) * boxPadding) , itemBoxPrefab.GetComponent<RectTransform>().sizeDelta.y + boxPadding) ;
        
        for(int i = 0; i < numberOfItems; i++)
        {
            RectTransform hold = Instantiate(itemBoxPrefab).GetComponent<RectTransform>();
            hold.parent = listBackgroundRectTransform;
            hold.anchoredPosition = new Vector3(boxPadding + (boxPadding * i) + (i * itemBoxPrefab.GetComponent<RectTransform>().sizeDelta.x), 0, 0);
            itemEntities[i] = hold;

            ChangeItemHudColor(i, 0);
        }
    
    }

}
