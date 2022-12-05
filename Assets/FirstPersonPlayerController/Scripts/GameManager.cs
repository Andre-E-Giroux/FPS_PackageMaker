using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void GoToBuildScene()
    {
        SceneManager.LoadScene("BuildScene");
    }

    public void GoToTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
