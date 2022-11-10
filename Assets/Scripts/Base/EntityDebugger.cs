using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void UpdateHealthBar(float health, float maxHealth)
    {
        float hold = (hpBarSizeMax / maxHealth) * health;

        healthBar.rectTransform.sizeDelta = new Vector2(hold, healthBar.rectTransform.sizeDelta.y);
    }
}
