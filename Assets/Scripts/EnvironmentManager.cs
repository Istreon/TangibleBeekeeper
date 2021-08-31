using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnvironmentManager : MonoBehaviour
{

    [SerializeField]
    InputActionAsset control;
    InputAction actionCalibration;


    private bool activate = true;

    [SerializeField]
    private GameObject env;

    [SerializeField]
    private Material sky;
    // Start is called before the first frame update
    void Start()
    {
        var keyboardActionMap = control.FindActionMap("KeyboardMap");
        actionCalibration = keyboardActionMap.FindAction("Environment");
        actionCalibration.performed += OnActivation;
        actionCalibration.Enable();

        SwapEnvironment();


    }

    void OnActivation(InputAction.CallbackContext context)
    {
        SwapEnvironment();
    }

    private void SwapEnvironment()
    {
        activate = !activate;

        env.SetActive(activate);

        if(activate)
        {
            RenderSettings.skybox = sky;
        } else
        {
            RenderSettings.skybox = null;
        }


    }

}
