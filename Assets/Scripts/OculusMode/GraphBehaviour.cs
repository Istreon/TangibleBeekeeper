using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class GraphBehaviour : MonoBehaviour
{
    public InputDeviceCharacteristics leftControllerCharacteristics;
    public GameObject leftControllerPrefab;
    public InputDevice leftDevice;

    public InputDeviceCharacteristics rightControllerCharacteristics;
    public GameObject rightControllerPrefab;
    public InputDevice rightDevice;

    private Slider timeSlider;
    private RingMenu orientationMenu;

    public BeesData beesData;
    public GraphAnchorsManager graphRotator;

    private bool isClicking  = false;

    
    

    // Start is called before the first frame update
    void Start()
    {
        timeSlider = RetrieveSlider(leftControllerPrefab);
        beesData.InitializeSlider(timeSlider);
        orientationMenu = RetrieveMenu(rightControllerPrefab);
        orientationMenu.callback = RotateGraph;

        SetDevice(leftDevice, leftControllerCharacteristics);
        SetDevice(rightDevice, rightControllerCharacteristics);
    }

    // Update is called once per frame
    void Update()
    {
        if(leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 lJoyValue))
        {
            if(lJoyValue.x > 0.5f)
            {
                timeSlider.value += 1;
                beesData.currentTurn += 1;
            }
            else if(lJoyValue.x < -0.5f)
            {
                timeSlider.value -= 1;
                beesData.currentTurn -= 1;
            }  
        }
        if(rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rJoyValue))
        {
            orientationMenu.SetJoystickPosition(rJoyValue);
            isClicking = true;
        }
        else
        {
            if(isClicking)
            {
                orientationMenu.wasClicked  = true;
                isClicking = false;
            }
            orientationMenu.SetJoystickPosition(Vector2.zero);
        }
    }

    private void SetDevice(InputDevice input, InputDeviceCharacteristics chara)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(chara, devices);
        if(devices.Count > 0)
        {
            input = devices[0];
        }
    }

    private Slider RetrieveSlider(GameObject prefab)
    {
        Canvas prefabCanvas = prefab.GetComponentInChildren<Canvas>();
        return prefabCanvas.GetComponentInChildren<Slider>();
    }

    private RingMenu RetrieveMenu(GameObject prefab)
    {
        Canvas prefabCanvas = prefab.GetComponentInChildren<Canvas>();
        return prefabCanvas.GetComponentInChildren<RingMenu>();
    }

    private void RotateGraph(string path)
    {
        graphRotator.keyPadPressed = int.Parse(path);
    }
}
