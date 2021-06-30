using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class SceneProgression : MonoBehaviour
{
    public InputDeviceCharacteristics rightCharacteristics;
    public InputDeviceCharacteristics leftCharacteristics;
    protected InputDevice rightDevice;
    protected InputDevice leftDevice;
    protected int sceneIndex;
    protected bool btnALastState = false;
    protected bool btnXLastState = false;
    protected bool isGoingBack = false;
    protected bool canContinue = true;
    public List<AudioSource> ambientSound;

    public LoadingScreen loadingScreen;
    public BlackBoxMode blackBox;
    protected bool isWaiting = false;
    protected bool isLoadingScene = false;

    protected int buildIndex;
    protected float pressedTime;
    protected bool skipTuto = false;

    private string sceneToLoad;
    private bool skipOnLoad = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Initialization of scene progression parameters
        blackBox.DisableBlackBoxMode();
        isWaiting = false;
        isLoadingScene = false;
        sceneIndex = 0;
        canContinue = true;

        //Initialization of right & left controllers
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(leftCharacteristics, devices);
        leftDevice = devices[0];
        Debug.Log("Left controller: " + leftDevice.name);

        devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightCharacteristics, devices);
        rightDevice = devices[0];
        Debug.Log("Right controller: " + rightDevice.name);

        //Retireving the build index of the current scene
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Each scene has its own progression
        //Method implemented in each daughter class
    }

    protected virtual void CanContinue()
    {
        sceneIndex += 1;
        canContinue = true;
    }

    protected virtual void GoBack()
    {
        sceneIndex -= 1;
        canContinue = true;
    }

    protected void CheckIfNext()
    {
        bool aPressed;
        Debug.Log("Entered CheckIfNext()");
        if(rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out aPressed) && aPressed)
        {
            if(!btnALastState)
            {
                pressedTime = Time.realtimeSinceStartup;
            }
            btnALastState = true;
            Debug.Log("\'A\' pressed");
        }
        else if (btnALastState)
        {
            btnALastState = false;
            CanContinue();
            Debug.Log("\'A\' released");
        }
        if(aPressed && Time.realtimeSinceStartup - pressedTime > 3.0f)
        {
            btnALastState = false;
            SkipInScene();
        } 
    }

    protected void CheckIfPrecedent()
    {
        Debug.Log("Entered CheckIfNext()");
        if(buildIndex > 0)
        {
            bool xPressed;
            if(leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out xPressed) && xPressed)
            {
                if(!btnXLastState)
                {
                    pressedTime = Time.realtimeSinceStartup;
                }
                btnXLastState  =true;
                Debug.Log("\'X\' pressed");
            }
            else if(btnXLastState)
            {
                btnXLastState = false;
                Debug.Log("\'X\' released");
                if(isWaiting && !isLoadingScene)
                {
                    ReturnToGame();
                }
                else if(!isWaiting && !isLoadingScene)
                {
                    GoBack();
                }
            }
            if(xPressed && Time.realtimeSinceStartup - pressedTime > 3.0f)
            {
                if(!isGoingBack)
                {
                    LoadPreviousScene();
                    btnXLastState = false;
                }
            } 
        }
    }

    public virtual void SkipOnLoad()
    {
        //Set a bool to skip the tutorial when the scene is loaded and restart the scene to ensure it
        skipTuto = true;
        //Start();
    }

    protected virtual void SkipInScene()
    {
        //Increment sceneIndex to skip the tutorial in the loaded scene
    }

    public void SetLoadingScreen(LoadingScreen loadingScreen)
    {
        this.loadingScreen = loadingScreen;
    }

    protected virtual void LoadNextScene()
    {
        foreach (AudioSource audio in ambientSound)
        {
            audio.Stop();
        }
        blackBox.EnableBlackBoxMode();
        loadingScreen.StartLoading(buildIndex + 1, false);
        isLoadingScene = true;
    }

    protected virtual void LoadPreviousScene()
    {
        isGoingBack = true;
        foreach (AudioSource audio in ambientSound)
        {
            audio.Stop();
        }
        blackBox.EnableBlackBoxMode();
        loadingScreen.StartLoading(buildIndex - 1, true);
        isLoadingScene = true;
    }

    protected virtual void GoToWaitingZone()
    {
        isWaiting = true;
        foreach (AudioSource audio in ambientSound)
        {
            audio.Pause();
        }
        blackBox.EnableBlackBoxMode();
    }

    protected virtual void ReturnToGame()
    {
        sceneIndex -= 1;
        canContinue = true;
        foreach (AudioSource audio in ambientSound)
        {
            audio.Play();
        }
        isWaiting = false;
        blackBox.DisableBlackBoxMode();
    }

    public void SetSceneToLoad(string sceneName)
    {
        sceneToLoad = sceneName;
    }

    public void SetSkipOnLoad(bool skip)
    {
        skipOnLoad = skip;
    }

    public void LoadScene()
    {
        if(!sceneToLoad.Equals(""))
        {
            foreach (AudioSource audio in ambientSound)
            {
                audio.Stop();
            }
            blackBox.EnableBlackBoxMode();
            isLoadingScene = true;
            loadingScreen.StartLoading(sceneToLoad, skipOnLoad);
        }
    }
}
