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


    public GameManager AddSceneManagerAssistants(ManagerAssistant assistant)
    {
        localManagerAssistants.Add(assistant);
        return this;
    }

    public void GoToSceneByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        localManagerAssistants.Clear();

        /*
        for (int i = 0; i < localManagerAssistants.Count; i++)
        {
            if (localManagerAssistants[i] == null)
            {
                Debug.LogError("Missing manager assistant at postion " + i);
                continue;
            }
            localManagerAssistants[i].StartManager();
        }

        for (int i = 0; i < disableObjectsOnLoad.Length; i++)
        {
            if (localManagerAssistants[i] == null)
            {
                Debug.LogError("Missing disable object on load at postion " + i);
                continue;
            }
            disableObjectsOnLoad[i].SetActive(false);
        }*/
    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
