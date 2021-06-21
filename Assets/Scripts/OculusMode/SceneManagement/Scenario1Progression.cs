using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Scenario1Progression : MonoBehaviour
{
    public InputDeviceCharacteristics rightCharacteristics;
    private InputDevice targetDevice;
    private int sceneIndex;
    private bool btnALastState = false;
    private bool canContinue = true;
    public BeesData dataModel;
    public GameObject endScreen;
    private AudioSource sceneAudio;
    public AudioSource ambientSound;
    public BlackBoxMode blackBox;
    private bool isWaiting = false;
    public LoadingScreen loadingScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightCharacteristics, devices);
        targetDevice = devices[0];
        Debug.Log("Using device " + targetDevice.name);

        endScreen.SetActive(false);
        blackBox.DisableBlackBoxMode();
        loadingScreen.gameObject.SetActive(false);

        sceneIndex = 0;
        canContinue = true;
        isWaiting = false;

        dataModel.NextScenario();

    }

    // Update is called once per frame
    void Update()
    {
        if(canContinue)
        {
            if(sceneIndex == 0)
            {
                canContinue = false;
                Debug.Log("Step 2: graph study");
                CheckIfNext();
            }
            else if(sceneIndex == 1)
            {
                canContinue = false;
                Debug.Log("End screen activated");
                endScreen.SetActive(true);
                blackBox.EnableBlackBoxMode();
                ambientSound.Stop();
            }
            else if(sceneIndex == 2)
            {
                canContinue = false;
                blackBox.EnableBlackBoxMode();
                endScreen.SetActive(false);
                loadingScreen.gameObject.SetActive(true);
                loadingScreen.StartLoading("LobbyScene");
            }
        }

        CheckIfNext();

    }

    public void CanContinue()
    {
        sceneIndex += 1;
        canContinue = true;
    }

    private void CheckIfNext()
    {
        //Debug.Log("Entered CheckIfNext()");
        if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
        {
            btnALastState = true;
            //Debug.Log("\'A\' pressed");
        }
        else if (btnALastState)
        {
            btnALastState = false;
            CanContinue();
            //Debug.Log("\'A\' released");
        } 
    }
}
