using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIControls : MonoBehaviour
{
    private GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        
    }

    /// <summary>
    /// Return to title scene
    /// </summary>
    public void ReturnToTitle()
    {
        gm.GoToSceneByString("TitleScene");
    }

}
