using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used during debug for the Entity class in effect
/// </summary>
public class EntityDebugger : MonoBehaviour
{
    [SerializeField]
    private bool isDebugModeOn = false;

    public GameObject debugCanvas;
    public UnityEngine.UI.RawImage healthBar;

    public GameObject player;

    private float hpBarSizeMax;

    // Start is called before the first frame update
    void Awake()
    {
        debugCanvas.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        debugCanvas.SetActive(isDebugModeOn);
        hpBarSizeMax = healthBar.rectTransform.sizeDelta.x ;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugCanvas.activeInHierarchy)
        {
            debugCanvas.transform.LookAt(player.transform);
        }
    }

    /// <summary>
    /// Update health bar above the entity
    /// </summary>
    /// <param name="health">current enitity health</param>
    /// <param name="maxHealth">Entities max health</param>
    public void UpdateHealthBar(float health, float maxHealth)
    {
        float hold = (hpBarSizeMax / maxHealth) * health;

        healthBar.rectTransform.sizeDelta = new Vector2(hold, healthBar.rectTransform.sizeDelta.y);

        if(healthBar.rectTransform.sizeDelta.x < 0)
            healthBar.rectTransform.sizeDelta = new Vector2(0, healthBar.rectTransform.sizeDelta.y);
    }
}
