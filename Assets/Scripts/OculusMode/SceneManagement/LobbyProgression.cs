using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LobbyProgression : MonoBehaviour
{
    public InputDeviceCharacteristics leftChara;
    private InputDevice leftDevice;
    public InputDeviceCharacteristics rightChara;
    private InputDevice rightDevice;
    public AudioSource ambientSound;
    public LoadingScreen loadingScreen;
    public BlackBoxMode blackBox;

    private bool isWaiting = true;
    private bool loadingScene = false;

    private bool lPBLastSate = false;
    private bool rPBLastState = false;
    private bool lSBLastState = false;
    private bool rSBLastState = false;
    private bool lGLastState = false;
    private bool rGLastState = false;
    private bool lTLastState = false;
    private bool rTLastState = false;

    // Start is called before the first frame update
    void Start()
    {
        loadingScreen.gameObject.SetActive(false);
        blackBox.DisableBlackBoxMode();
        isWaiting = true;
        loadingScene = false;

        List<InputDevice> devices = new List<InputDevice>();
        /*InputDevices.GetDevicesWithCharacteristics(leftChara, devices);
        leftDevice = devices[0];*/

        devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightChara, devices);
        rightDevice = devices[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(isWaiting && !loadingScene)
        {
            /*if(leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool lPrimaryBtn) && lPrimaryBtn)
            {
                lPBLastSate = true;
            }
            else if(lPBLastSate) 
            {
                isWaiting = false;
                lPBLastSate = false;
            }*/
            if(rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool rPrimaryBtn) && rPrimaryBtn)
            {
                rPBLastState = true;
            }
            else if(rPBLastState) 
            {
                isWaiting = false;
                rPBLastState = false;
            }
            /*if(leftDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool lSecondaryBtn) && lSecondaryBtn)
            {
                lSBLastState = true;
            }
            else if(lSBLastState)
            {
                isWaiting = false;
                lSBLastState = false;
            }
            if(rightDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rSecondaryBtn) && rSecondaryBtn)
            {
                rSBLastState = true;
            }
            else if(rSBLastState)
            {
                isWaiting = false;
                rSBLastState = false;
            }
            if(leftDevice.TryGetFeatureValue(CommonUsages.grip, out float lGrip) && lGrip > 0.5f)
            {
                lGLastState = true;
            }
            else if(lGLastState)
            {
                isWaiting = false;
                lGLastState = false;
            }
            if(rightDevice.TryGetFeatureValue(CommonUsages.grip, out float rGrip) && rGrip > 0.5f)
            {
                rGLastState = true;
            }
            else if(rGLastState)
            {
                isWaiting = false;
                rGLastState = false;
            }
            if(leftDevice.TryGetFeatureValue(CommonUsages.trigger, out float lTrigger) && lTrigger > 0.5f)
            {
                lTLastState = true;
            }
            else if(lTLastState)
            {
                isWaiting = false;
                lTLastState = false;
            }
            if(rightDevice.TryGetFeatureValue(CommonUsages.trigger, out float rTrigger) && rTrigger > 0.5f)
            {
                rTLastState = true;
            }
            else if(rTLastState)
            {
                isWaiting = false;
                rTLastState = false;
            }*/
        }
        
        if(!isWaiting && !loadingScene)
        {
            ambientSound.Stop();
            loadingScreen.gameObject.SetActive(true);
            blackBox.EnableBlackBoxMode();
            loadingScreen.StartLoading("DivisionScene");
            loadingScene = true;
        }
    }
}
