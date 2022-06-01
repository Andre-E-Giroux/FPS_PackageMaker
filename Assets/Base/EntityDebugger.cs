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

    // Start is called before the first frame update
    void Awake()
    {
        debugCanvas.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        debugCanvas.SetActive(isDebugModeOn);
    }

    // Update is called once per frame
    void Update()
    {
        if (debugCanvas.activeInHierarchy)
        {
            debugCanvas.transform.LookAt(player.transform);
        }
    }
}
