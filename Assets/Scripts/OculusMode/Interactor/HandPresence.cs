using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool showController;
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject controllerPrefab;
    public GameObject handModelPrefab;
    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator controllerAnimator;
    private Animator handAnimator;


    // Start is called before the first frame update
    void Start()
    {
        //showController = false;
        TryInitialyze();
    }

    void TryInitialyze()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        if(devices.Count > 0){
            targetDevice = devices[0];
            spawnedController = Instantiate(controllerPrefab, transform);
            controllerAnimator = spawnedController.GetComponent<Animator>();
            
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        if(!targetDevice.isValid)
        {
            TryInitialyze();
        }
        else
        {
            if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                handAnimator.SetFloat("Pinch", triggerValue);
            }
            else
            {
                handAnimator.SetFloat("Pinch", 0.0f);
            }
            
            if(targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                handAnimator.SetFloat("Flex", gripValue);
            }
            else
            {
                handAnimator.SetFloat("Flex", 0.0f);
            }
        }
           
    }

    void UpdateControllerAnimation()
    {
        if(!targetDevice.isValid)
        {
            TryInitialyze();
        }
        else
        {
            if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonOne) && buttonOne)
            {
                controllerAnimator.SetFloat("Button 1", 1.0f);
            }
            else
            {
                controllerAnimator.SetFloat("Button 1", 0.0f);
            }
            if(targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool buttonTwo) && buttonTwo)
            {
                controllerAnimator.SetFloat("Button 2", 1.0f);
            }
            else
            {
                controllerAnimator.SetFloat("Button 2", 0.0f);
            }
            if(targetDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool buttonThree) && buttonThree)
            {
                controllerAnimator.SetFloat("Button 3", 1.0f);
            }
            else
            {
                controllerAnimator.SetFloat("Button 3", 0.0f);
            }

            if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                controllerAnimator.SetFloat("Trigger", triggerValue);
            }
            else
            {
                controllerAnimator.SetFloat("Trigger", 0.0f);
            }
            if(targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                controllerAnimator.SetFloat("Grip", gripValue);
            }
            else
            {
                controllerAnimator.SetFloat("Grip", 0.0f);
            }

            if(targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickValue))
            {
                controllerAnimator.SetFloat("Joy X", joystickValue.x);
                controllerAnimator.SetFloat("Joy Y", joystickValue.y);
            }
            else
            {
                controllerAnimator.SetFloat("Joy X", 0.0f);
                controllerAnimator.SetFloat("Joy Y", 0.0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(showController)
        {
            spawnedController.SetActive(true);
            spawnedHandModel.SetActive(false);
            UpdateControllerAnimation();
        }
        else
        {
            spawnedController.SetActive(false);
            spawnedHandModel.SetActive(true);
            UpdateHandAnimation();
        }

        /*if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonOne) && buttonOne)
        {
            showController = true;
        }
        if(targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool buttonTwo) && buttonTwo)
        {
            showController = false;
        }*/

    }
}