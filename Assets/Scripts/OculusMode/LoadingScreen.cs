using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private AsyncOperation loadingOperation;
    public TMPro.TextMeshProUGUI percentage;
    public Slider slider;
    
    public void StartLoading(string scene)
    {
        loadingOperation = SceneManager.LoadSceneAsync(scene);
    }


    void Update()
    {
        if(!loadingOperation.isDone)
        {
            slider.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            percentage.text = Mathf.Round(progressValue * 100) + " %";
        }
    }
}
