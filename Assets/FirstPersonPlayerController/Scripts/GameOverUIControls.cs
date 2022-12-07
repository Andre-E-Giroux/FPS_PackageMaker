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


    public void ReturnToTitle()
    {
        gm.GoToTitleScene();
    }

}
