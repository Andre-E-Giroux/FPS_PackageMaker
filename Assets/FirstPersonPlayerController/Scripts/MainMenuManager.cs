using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    private GameManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = FindObjectOfType<GameManager>();
    }



    public void GoToSceneByString(string sceneName)
    {
        if (gManager)
            gManager.GoToSceneByString(sceneName);
        else
            Debug.LogError("Game Manager not found at Start");
    }
}
