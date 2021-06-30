using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance = null;
    private AsyncOperation loadingOperation;

    public Canvas mainScreen;
    public TMPro.TextMeshProUGUI percentage;
    public Slider progressBar;

    private static bool skipTuto = false;
    
    private void Awake()
    {
        mainScreen.worldCamera = Camera.main;
        mainScreen.renderMode = RenderMode.ScreenSpaceCamera;

        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            Destroy(gameObject);   
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        mainScreen.gameObject.SetActive(false);
    }
    public void StartLoading(int index, bool skip)
    {
        loadingOperation = SceneManager.LoadSceneAsync(index);
        skipTuto = skip;
        mainScreen.gameObject.SetActive(true);
        mainScreen.worldCamera = Camera.main;
        mainScreen.renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void StartLoading(string sceneName, bool skip)
    {
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        skipTuto = skip;
        mainScreen.gameObject.SetActive(true);
        mainScreen.worldCamera = Camera.main;
        mainScreen.renderMode = RenderMode.ScreenSpaceCamera;
    }


    private void Update()
    {
        if(loadingOperation != null)
        {
            if(!loadingOperation.isDone)
            {
                mainScreen.gameObject.SetActive(true);
                progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
                float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
                percentage.text = Mathf.Round(progressValue * 100) + " %";
            }
            else
            {
                mainScreen.gameObject.SetActive(false);
                GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
                if(sceneManager.TryGetComponent<SceneProgression>(out SceneProgression sceneProgression))
                {
                    sceneProgression.SetLoadingScreen(this);
                    if(skipTuto)
                    {
                        sceneProgression.SkipOnLoad();
                    }
                }
                
                loadingOperation = null;
            }
        }
        

    }
}
