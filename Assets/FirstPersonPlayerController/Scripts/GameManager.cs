using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    public List<ManagerAssistant> localManagerAssistants = new List<ManagerAssistant>();

    public GameObject[] disableObjectsOnLoad;

    private static GameManager instance;
    //private static string  titleSceneName = "TitleScene";
    // private string buildSceneName = "BuildScene";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Add ManagerAssistants from the scene
    /// </summary>
    /// <param name="assistant">assistant to be added</param>
    /// <returns>The GameManager instance</returns>
    public GameManager AddSceneManagerAssistants(ManagerAssistant assistant)
    {
        localManagerAssistants.Add(assistant);
        return this;
    }

    /// <summary>
    /// Go scene that is called by the string input
    /// </summary>
    /// <param name="sceneName">name of the scene to be moved to</param>
    public void GoToSceneByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        localManagerAssistants.Clear();
    }

    /// <summary>
    /// Quit the game, and close it
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

}
