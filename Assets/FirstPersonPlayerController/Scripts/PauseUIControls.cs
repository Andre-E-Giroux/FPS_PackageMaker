using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to manage pause menu UI interactions
/// </summary>
public class PauseUIControls : MonoBehaviour
{
    private GameManager gm;

    [SerializeField]
    private PlayerSM playerSM;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();

    }

    /// <summary>
    /// UnPause button action
    /// </summary>
    public void UnPauseButton()
    {
        playerSM.PauseGame();
    }

    /// <summary>
    /// Return to title scene, button action
    /// </summary>
    public void ReturnToTitle()
    {
        gm.GoToSceneByString("TitleScene");
    }





}
