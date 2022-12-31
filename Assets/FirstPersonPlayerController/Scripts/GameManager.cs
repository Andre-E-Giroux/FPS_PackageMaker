using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    private List<ManagerAssistant> managerAssistants = new List<ManagerAssistant>();

    public GameObject[] disableObjectsOnLoad;

    private GameObject instance;
    //private static string  titleSceneName = "TitleScene";
    // private string buildSceneName = "BuildScene";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            instance = this.gameObject;
        }
        else
        {
            instance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    public GameManager AddSceneManagerAssistants(ManagerAssistant assistant)
    {
        managerAssistants.Add(assistant);
        return this;
    }

    public void GoToSceneByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);


        for(int i = 0; i < managerAssistants.Count; i++)
        {
            managerAssistants[i].StartManager();
        }

        for (int i = 0; i < disableObjectsOnLoad.Length; i++)
        {
            disableObjectsOnLoad[i].SetActive(false);
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
