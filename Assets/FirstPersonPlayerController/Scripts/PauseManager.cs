using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private GameManager gm;

    [SerializeField]
    private PlayerSM playerSM;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();

    }

    public void UnPauseButton()
    {
        playerSM.PauseGame();

    }

    public void ReturnToTitle()
    {
        gm.GoToSceneByString("TitleScene");
    }





}
