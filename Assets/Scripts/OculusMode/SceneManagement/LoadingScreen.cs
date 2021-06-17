using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private AsyncOperation loadingOperation;
    public Canvas mainScreen;
    public TMPro.TextMeshProUGUI percentage;
    public Slider progressBar;
    
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        //mainScreen.renderMode = RenderMode.ScreenSpaceCamera;
        //mainScreen.worldCamera = Camera.main;
    }
    public void StartLoading(string scene)
    {
        loadingOperation = SceneManager.LoadSceneAsync(scene);
    }


    private void Update()
    {
        if(!loadingOperation.isDone)
        {
            progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            percentage.text = Mathf.Round(progressValue * 100) + " %";
        }
    }
}
